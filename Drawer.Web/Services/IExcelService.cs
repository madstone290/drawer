using ClosedXML.Excel;

namespace Drawer.Web.Services
{
    public interface IExcelService
    {
        /// <summary>
        /// 엑셀 테이블을 읽고 T타입 인스턴스로 변환한다.
        /// </summary>
        /// <typeparam name="T">인스턴스 타입</typeparam>
        /// <param name="buffer">엑셀 데이터</param>
        /// <returns></returns>
        IEnumerable<T> ReadTable<T>(byte[] buffer, ExcelOptions? options = null);

        /// <summary>
        /// 엑셀 테이블을 생성한다.
        /// </summary>
        /// <typeparam name="T">인스턴스 타입</typeparam>
        /// <param name="list">인스턴스 리스트</param>
        /// <returns></returns>
        byte[] WriteTable<T>(IEnumerable<T> list, ExcelOptions? options = null);
    }

    /// <summary>
    /// 엑셀 옵션
    /// </summary>
    /// <param name="Columns"></param>
    public record ExcelOptions(IEnumerable<ExcelColumn> Columns);

    /// <summary>
    /// 엑셀컬럼
    /// </summary>
    /// <param name="Name">컬럼명</param>
    /// <param name="Caption">캡션</param>
    public record ExcelColumn(string Name, string Caption);

    public class ExcelOptionsBuilder
    {
        private readonly List<ExcelColumn> columns = new();

        public ExcelOptionsBuilder AddColumn(string name, string caption)
        {
            columns.Add(new ExcelColumn(name, caption));
            return this;
        }

        public ExcelOptions Build()
        {
            return new ExcelOptions(columns);
        }
    }

    public class ExcelService : IExcelService
    {
        public IEnumerable<T> ReadTable<T>(byte[] buffer, ExcelOptions? options = null)
        {
            var list = new List<T>();
            using var stream = new MemoryStream(buffer);
            var workBook = new XLWorkbook(stream);
            var worksheet = workBook.Worksheet(1);

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);

            IEnumerable<ExcelColumn> columns = options?.Columns
              ?? properties.Select(prop => new ExcelColumn(prop.Name, prop.Name)).ToList();

            var sheetColumns = worksheet.ColumnsUsed();
            // key: 컬럼캡션, value: 컬럼번호
            var columnCaptionNumbers = new Dictionary<string, int>();
            bool firstRow = true;
            foreach (var row in worksheet.RowsUsed())
            {
                // 첫 로우는 컬럼 헤더
                if (firstRow)
                {
                    // 컬럼을 식별하기위해 셀의 컬럼번호와 텍스트값을 읽는다
                    foreach (var column in sheetColumns)
                    {
                        var headerCell = row.Cell(column.ColumnNumber());
                        columnCaptionNumbers.Add(Normalize(headerCell.GetString()), headerCell.Address.ColumnNumber);
                    }
                    firstRow = false;
                    continue;
                }

                var instance = (T)Activator.CreateInstance(typeof(T))!;
                foreach (var column in columns)
                {
                    if (columnCaptionNumbers.TryGetValue(Normalize(column.Caption), out int columnNumber))
                    {
                        var property = properties.FirstOrDefault(x => string.Equals(x.Name, column.Name, StringComparison.OrdinalIgnoreCase));
                        if (property != null)
                        {
                            try
                            {
                                var properValue = Convert.ChangeType(row.Cell(columnNumber).Value, property.PropertyType);
                                property.SetValue(instance, properValue);
                            }
                            catch { } // Failed to change value type 
                        }
                    }
                }

                list.Add(instance);
            }

            return list;
        }

        public byte[] WriteTable<T>(IEnumerable<T> list, ExcelOptions? options = null)
        {
            var wbook = new XLWorkbook();
            var worksheet = wbook.AddWorksheet();

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);

            IEnumerable<ExcelColumn> columns = options?.Columns
                ?? properties.Select(prop => new ExcelColumn(prop.Name, prop.Name)).ToList();

            // Header 생성
            var rowNumber = 1;
            CreateHeaders(worksheet, columns, rowNumber);

            rowNumber = 2;
            // DataRow 생성
            foreach (var instance in list)
            {
                var columnNumber = 1;
                foreach (var column in columns)
                {
                    var property = properties.FirstOrDefault(x => string.Equals(x.Name, column.Name, StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        var cell = worksheet.Cell(rowNumber, columnNumber);
                        var value = property.GetValue(instance);
                        cell.SetValue(value);
                    }
                    columnNumber++;
                }
                rowNumber++;
            }
            using var stream = new MemoryStream();
            wbook.SaveAs(stream);

            return stream.ToArray();
        }

        private void CreateHeaders(IXLWorksheet worksheet, IEnumerable<ExcelColumn> columns, int rowNumber)
        {
            int columnNumber = 1;
            foreach (var column in columns)
            {
                worksheet.Cell(rowNumber, columnNumber).Value = column.Caption;
                columnNumber++;
            }
        }

        /// <summary>
        /// 컬럼 문자열을 표준화한다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Normalize(string value)
        {
            return value.ToLower();
        }

    }
}
