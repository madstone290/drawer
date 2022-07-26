using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.AidBlazor
{
    public class FlexSpace
    {
        private FlexSpace(string @class)
        {
            Class = @class;
        }

        public string Class { get; }

        /// <summary>
        /// 플렉스 컨테이너에서 자기 영역을 유지한다
        /// </summary>
        public static FlexSpace Stay { get; } = new FlexSpace("aid-flex-stay");

        /// <summary>
        /// 플렉스 컨테이너에서 컬럼방향으로 남은 영역을 모두 채운다
        /// </summary>
        public static FlexSpace ColumnFill { get; } = new FlexSpace("aid-flex-column-fill");

        /// <summary>
        /// 플렉스 컨테이너에서 로우방향으로 남은 영역을 모두 채운다
        /// </summary>
        public static FlexSpace RowFill { get; } = new FlexSpace("aid-flex-row-fill"); 
    }

    
}
