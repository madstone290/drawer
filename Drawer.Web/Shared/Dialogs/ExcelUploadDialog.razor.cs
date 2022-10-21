using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Drawer.Web.Shared.Dialogs
{
    public partial class ExcelUploadDialog
    {
        private IBrowserFile? _file;

        [CascadingParameter]
        public MudDialogInstance Dialog { get; private set; } = null!;

        private void UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
        }

        private void Cancel_Click()
        {
            Dialog.Cancel();
        }

        private async Task Upload_ClickAsync()
        {
            if(_file != null)
            {
                var buffer = new byte[_file.Size];
                await _file.OpenReadStream(_file.Size).ReadAsync(buffer);

                Dialog.Close(buffer);
            }
        }
    }
}
