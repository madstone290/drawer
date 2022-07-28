function UseTableResize(className) {
    var tables = document.getElementsByClassName(className);
    for (var i = 0; i < tables.length; i++) {
        makeTableResize(tables[i]);
    }
}

function makeTableResize(table) {
    let row = table.getElementsByTagName('tr')[0];
    let cols = row ? row.children : undefined;
    if (!cols) return;

    var divArray = new Array(cols.length);

    // div 생성
    for (var i = 0; i < cols.length; i++) {
        // div position: absolute 적용을 위해서 
        cols[i].style.position = 'relative';

        let div = document.createElement('div');
        div.style.top = 0;
        div.style.right = 0;
        div.style.width = '5px';
        div.style.position = 'absolute';
        div.style.cursor = 'col-resize';
        div.style.userSelect = 'none';
        div.style.height = table.offsetHeight + 'px';

        cols[i].appendChild(div);

        setListeners(div);

        divArray[i] = div;
    }

    // table 높이 변경시 div 높이 변경
    function resizeDiv() {
        for (var i = 0; i < divArray.length; i++) {
            divArray[i].style.height = table.offsetHeight + 'px';
        }
    }

    var observe = new ResizeObserver(resizeDiv);
    observe.observe(table);

    function setListeners(div) {
        var pageX, leftColumn, leftColumnWidth, tableWidth;

        // 초기값 설정
        div.addEventListener('mousedown', function (e) {

            pageX = e.pageX;
            tableWidth = table.offsetWidth;
            leftColumn = e.target.parentElement;
            
            var padding = paddingDiff(leftColumn);
            leftColumnWidth = leftColumn.offsetWidth - padding;
        });

        div.addEventListener('mouseover', function (e) {
            e.target.style.borderRight = '2px solid #0000ff';
        })

        div.addEventListener('mouseout', function (e) {
            e.target.style.borderRight = '';
        })

        document.addEventListener('mousemove', function (e) {
            if (leftColumn) {
                var diffX = e.pageX - pageX;

                leftColumn.style.width = (leftColumnWidth + diffX) + 'px';

                table.style.width = (tableWidth + diffX) + 'px';
            }
        });


        // 값 해제
        document.addEventListener('mouseup', function (e) {
            pageX = undefined;
            leftColumn = undefined;
            leftColumnWidth = undefined
            tableWidth = undefined;
        });
    }
    
    function paddingDiff(col) {

        if (getStyleVal(col, 'box-sizing') == 'border-box') {
            return 0;
        }

        var padLeft = getStyleVal(col, 'padding-left');
        var padRight = getStyleVal(col, 'padding-right');
        return (parseInt(padLeft) + parseInt(padRight));

    }

    function getStyleVal(elm, css) {
        return (window.getComputedStyle(elm, null).getPropertyValue(css))
    }
}
