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
        IEnumerable<T> ReadTable<T>(byte[] buffer);

        /// <summary>
        /// 엑셀 테이블을 생성한다.
        /// </summary>
        /// <typeparam name="T">인스턴스 타입</typeparam>
        /// <param name="list">인스턴스 리스트</param>
        /// <returns></returns>
        byte[] WriteTable<T>(IEnumerable<T> list);
    }

    public class ExcelService : IExcelService
    {
        public IEnumerable<T> ReadTable<T>(byte[] buffer)
        {
            using var stream = new MemoryStream(buffer);
            var wbook = new XLWorkbook(stream);

            return ReadTable<T>(wbook);
        }

        public byte[] WriteTable<T>(IEnumerable<T> list)
        {
            var wbook = new XLWorkbook();
            var worksheet = wbook.AddWorksheet();

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);
            
            var rowNumber = 1;
            var columnNumber = 1;
            // Create Header Cells
            foreach (var property in properties)
            {
                worksheet.Cell(rowNumber, columnNumber).Value = property.Name;
                columnNumber++;
            }
            rowNumber++;

            // Create Data Cells
            foreach (var instance in list)
            {
                columnNumber = 1;
                foreach (var property in properties)
                {
                    worksheet.Cell(rowNumber, columnNumber).Value = property.GetValue(instance);
                    columnNumber++;
                }
                rowNumber++;
            }
            using var stream = new MemoryStream();
            wbook.SaveAs(stream);

            return stream.ToArray();
        }

        private static List<T> ReadTable<T>(XLWorkbook workBook)
        {
            // 1번 시트만 사용한다
            var worksheet = workBook.Worksheet(1);

            var columns = worksheet.ColumnsUsed();
            // key: 컬럼명, value: 컬럼번호
            var columnNameNumbers = new Dictionary<string, int>();
            bool firstRow = true;

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);

            var list = new List<T>();
            foreach (var row in worksheet.RowsUsed())
            {
                // 첫 로우는 컬럼 헤더
                if (firstRow)
                {
                    foreach (var column in columns)
                    {
                        var headerCell = row.Cell(column.ColumnNumber());
                        columnNameNumbers.Add(headerCell.GetString().ToLower(), headerCell.Address.ColumnNumber);
                    }
                    firstRow = false;
                    continue;
                }

                var instance = (T)Activator.CreateInstance(typeof(T))!;
                foreach (var property in properties)
                {
                    if (columnNameNumbers.TryGetValue(property.Name.ToLower(), out int columnNumber))
                    {
                        var rawValue = row.Cell(columnNumber).Value;
                        try
                        {
                            var properValue = Convert.ChangeType(rawValue, property.PropertyType);
                            property.SetValue(instance, properValue);
                        }
                        catch
                        {
                            // Failed to change value type
                        }
                    }
                }

                list.Add(instance);
            }

            return list;
        }

    
    }
}
