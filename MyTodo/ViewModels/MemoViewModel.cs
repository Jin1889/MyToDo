
using MyTodo.Service;
using MyTodo.Views;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.ViewModels
{
    public class MemoViewModel : NavigationViewModel
    {

        public MemoViewModel(IMemoService service, IContainerProvider provider) : base(provider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            DeleteCommand = new DelegateCommand<MemoDto>(Detele);
            this.service = service;
            GetDataAsync();
        }

        private async void Detele(MemoDto dto)
        {
            try
            {
                UpdateLoading(true);
                var deleteResult = await service.DeleteAsync(dto.Id);
                if (deleteResult.Status)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (model != null)
                        MemoDtos.Remove(model);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": GetDataAsync(); break;
                case "保存": Save(); break;
            }
        }


        //搜索
        private string search;

        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }


        private bool isRightDrawerOpen;
        //右侧编辑窗口是否展开
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }
        private MemoDto currentDto;

        //编辑选中/新增时对象
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }


        //添加备忘录
        private void Add()
        {
            CurrentDto = new MemoDto();
            IsRightDrawerOpen = true;
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Title) || string.IsNullOrWhiteSpace(CurrentDto.Content))
            {
                return;
            }
            UpdateLoading(true);
            try
            {
                if (CurrentDto.Id > 0)
                {
                    var updateResult = await service.UpdateAsync(CurrentDto);
                    if (updateResult.Status)
                    {
                        var memo = MemoDtos.FirstOrDefault(t => t.Id == CurrentDto.Id);
                        if (memo != null)
                        {
                            memo.Title = CurrentDto.Title;
                            memo.Content = CurrentDto.Content;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await service.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
                        isRightDrawerOpen = false;
                        RaisePropertyChanged(nameof(isRightDrawerOpen));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void Selected(MemoDto dto)
        {
            try
            {
                UpdateLoading(true);
                var memoResult = await service.GetFirstOfDefaultAsync(dto.Id);
                if (memoResult.Status)
                {
                    CurrentDto = memoResult.Result;
                    IsRightDrawerOpen = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }

        private readonly IMemoService service;
        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        async void GetDataAsync()
        {
            UpdateLoading(true);

            var memoResult = await service.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });
            if (memoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in memoResult.Result.Items)
                {
                    memoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataAsync();
        }
    }
}
