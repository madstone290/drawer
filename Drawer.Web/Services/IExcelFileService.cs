﻿using Drawer.Web.Shared.Dialogs;
using MudBlazor;

namespace Drawer.Web.Services
{
    public interface IExcelFileService
    {
        /// <summary>
        /// 엑셀파일을 다운로드한다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        Task Download<T>(string fileName, IEnumerable<T> list, ExcelOptions? options = null);

        /// <summary>
        /// 엑셀파일을 업로드한다.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> Upload<T>(ExcelOptions? options = null);
    }

    public class ExcelFileService : IExcelFileService
    {
        private readonly IExcelService _excelService;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;

        public ExcelFileService(IExcelService excelService, IFileService fileService, IDialogService dialogService)
        {
            _excelService = excelService;
            _fileService = fileService;
            _dialogService = dialogService;
        }

        public async Task Download<T>(string fileName, IEnumerable<T> list, ExcelOptions? options = null)
        {
            var buffer = _excelService.WriteTable(list, options);
            var stream = new MemoryStream(buffer);

            await _fileService.DownloadAsync(fileName, stream);
        }

        public async Task<IEnumerable<T>> Upload<T>(ExcelOptions? options = null)
        {
            var dialogOptions = new DialogOptions()
            {
                MaxWidth = MaxWidth.Small,
            };
            var dialogParameters = new DialogParameters
            {
            };
            var dialog = _dialogService.Show<ExcelUploadDialog>(null, options: dialogOptions, parameters: dialogParameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                byte[] buffer = (byte[])result.Data;

            return new ExcelService().ReadTable<T>(buffer, options);
            }
            else
            {
                return Enumerable.Empty<T>();
            }
        }

    }
}
