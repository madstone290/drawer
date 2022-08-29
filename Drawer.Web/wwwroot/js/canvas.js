/** fabric.Canvas prototypes **/
fabric.Canvas.prototype.getItemById = function (id) {
    let objects = this.getObjects();

    for (const obj of objects) {
        if (obj.id === id)
            return obj;
    }
    return null;
};

/** fabric.Group prototypes **/
fabric.Group.prototype.setBackColor = function (backColor) {
    this.backColor = backColor;
    let shapeItem = this.item(0);
    shapeItem.set({
        fill: backColor
    });
};


fabric.Group.prototype.setFontSize = function (fontSize) {
    this.fontSize = fontSize;
    let textItem = this.item(1);
    textItem.set({
        fontSize: this.fontSize
    });
    this.refresh();
};

/**
 * 텍스트를 설정한다.
 * @param {any} text 텍스트
 */
fabric.Group.prototype.setText = function (text) {
    this.text = text;
    this.refreshText();
};

/**
 * 텍스트 각도를 설정한다.
 * @param {any} degree row(수평), column(수직) 중 하나
 */
fabric.Group.prototype.setDegree = function (degree) {
    this.degree = degree;
    this.refreshText();
}

/**
 * 텍스트 렌더링을 갱신한다.
 * */
fabric.Group.prototype.refreshText = function () {
    let textItem = this.item(1);

    let text = this.text ? this.text : "";
    if (this.degree == "column") {
        text = text.split("").join("\n");
    }

    textItem.set({
        text: text
    });
    this.refresh();
}

/**
 * 수평 정렬을 설정한다.
 * @param {any} hAlignment left, center, top 중 하나
 */
fabric.Group.prototype.setHAlignment = function (hAlignment) {
    this.hAlignment = hAlignment;
    this.refreshHAlignment();
}

/**
 * 수평 정렬 렌더링을 갱신한다.
 * */
fabric.Group.prototype.refreshHAlignment = function () {
    let textItem = this.item(1);

    let groupScaledWidth = this.getScaledWidth();
    let textScaledWidth = textItem.getScaledWidth();

    let textLeft = -1 * (groupScaledWidth / 2);
    switch (this.hAlignment) {
        case "left":
            textLeft = textLeft;
            break;
        case "right":
            textLeft = textLeft + groupScaledWidth - textScaledWidth;
            break;
        case "center":
        default:
            textLeft = textLeft + (groupScaledWidth / 2) - (textScaledWidth / 2);
            break;
    }
    textItem.set({
        left: textLeft
    });
}

/**
 * 수직 정렬을 설정한다.
 * @param {string} vAlignment  top, center, bottom 중 하나
 */
fabric.Group.prototype.setVAlignment = function (vAlignment) {
    this.vAlignment = vAlignment;
    this.refreshVAlignment();
}

/**
 * 수직 정렬 렌더링을 갱신한다.
 * */
fabric.Group.prototype.refreshVAlignment = function () {
    let textItem = this.item(1);

    let groupScaledHeight = this.getScaledHeight();
    let textScaledHeight = textItem.getScaledHeight();

    let textTop = -1 * (groupScaledHeight / 2);
    switch (this.vAlignment) {
        case "top":
            textTop = textTop;
            break;
        case "bottom":
            textTop = textTop + groupScaledHeight - textScaledHeight;
            break;
        case "center":
        default:
            textTop = textTop + (groupScaledHeight / 2) - (textScaledHeight / 2);
            break;
    }

    textItem.set({
        top: textTop
    });
}

/**
 * 그룹의 속성값에 맞게 현재 렌더링 상태를 갱신한다.
 * */
fabric.Group.prototype.refresh = function () {
    // 아이템을 제거한 후 위치/사이즈 갱신 후 삽입
    let shapeItem = this.item(0);
    let textItem = this.item(1);

    // 그룹의 크기는 직접적으로 변경되거나 텍스트의 변경으로 인해 간접적으로 변경될 수 있다.
    let groupScaledHeight = this.getScaledHeight();
    let groupScaledWidth = this.getScaledWidth();
    let textScaledWidth = textItem.getScaledWidth();
    let textScaledHeight = textItem.getScaledHeight();

    let nextWidth = Math.max(groupScaledWidth, textScaledWidth);
    let nextHeight = Math.max(groupScaledHeight, textScaledHeight);
    
    this.remove(shapeItem);
    this.remove(textItem);
    
    shapeItem.set({
        left: this.left,
        top: this.top,
        scaleX: (nextWidth - shapeItem.strokeWidth) / shapeItem.width,
        scaleY: (nextHeight - shapeItem.strokeWidth) / shapeItem.height,
    });

    textItem.set({
        left: this.left,
        top: this.top
    });

    this.addWithUpdate(shapeItem);
    this.addWithUpdate(textItem);

    // 정렬 상태 복원
    this.refreshHAlignment();
    this.refreshVAlignment();
}

fabric.Group.prototype.toItemInfo = function () {
    let itemInfo = new ItemInfo();
    itemInfo.itemId = this.id;
    itemInfo.type = this.type;
    itemInfo.left = this.left;
    itemInfo.top = this.top;
    itemInfo.width = parseInt(this.width.toFixed());
    itemInfo.height = parseInt(this.height.toFixed());
    itemInfo.isPattern = this.isPattern;
    itemInfo.patternImageId = this.patternImageId;
    itemInfo.backColor = this.backColor;
    itemInfo.shape = this.shape;
    itemInfo.hAlignment = this.hAlignment;
    itemInfo.vAlignment = this.vAlignment;
    itemInfo.degree = this.degree;
    itemInfo.text = this.text;
    itemInfo.fontSize = this.fontSize;

    return itemInfo;
};

/** 일반 함수 **/
function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

class ItemInfo {
    itemId = "";
    type = "group";
    shape = "rect";
    left = 0;
    top = 0;
    width = 100;
    height = 100;

    isPattern = false;
    backColor = "#ADD8E6";
    patternImageId = "";

    fontSize = 15;
    text = "";
    vAlignment = "center";
    hAlignment = "center";
    degree = "row";
};

/**
 * 캔버스에 레이아웃을 그릴수 있게 한다.
 * 모든 캔버스 아이템은 그룹아이템이며 하나의 Shape(Rect, Circle)와 하나의 Text 오브젝트로 구성된다.
 * */
class Drawer {
    canvas; // fabrict canvas instance

    // 캔버스는 고정 크기를 사용한다. 실제 크기 조절은 줌 기능을 이용한다.
    canvasWidth = 1920;
    canvasHeight = 1080;

    // 캔버스 줌 레벨
    zoomLevel = 1;

    // event callback functions 
    itemSelectionChanged = function (id) {
    };

    copiedItemInfo = null;

    colors = {
        fill: "#ADD8E6",
        stroke: "#ADD8E6",
        border: "#FF0000",
        corner: "#FF0000",
        cornerStroke: "#000000"
    }

    // 텍스트 박스 top/bottom 마진
    textboxMargin = 10;

    gridSize = 20;
    itemSize = 100;

    // 쉐이프 배경 이미지의 원본을 담고 있는 canvas 요소 딕셔너리
    sourceCanvasElementDict = {}

    getImage;



    constructor(canvasId) {
        const canvasElement = document.getElementById(canvasId);
        this.canvas = this.createFabricCanvas(canvasElement);
    }

    createFabricCanvas(canvasElement) {
        let canvas = new fabric.Canvas(canvasElement, {
            selection: false // 그룹 선택 불가
        });

        this.addCanvasSelectionEventHandler(canvas);
        this.addCanvasObjectModifiedHandler(canvas, this.gridSize, this.gridSize);
        this.addCanvasDeleteKeyEventHandler(canvas);
        this.addCanvasCtrlCVKeyEventHandler(canvas);
        this.addDropEventHandler(canvas)

        return canvas;
    }

    /**
     * 캔버스 아이템 선택/해제 이벤트 핸들러를 추가한다.
     * @param {any} canvas fabric canvas
     */
    addCanvasSelectionEventHandler(canvas) {
        let self = this;
        canvas.on("selection:cleared", function (e) {
            self.itemSelectionChanged();
        });
        canvas.on("selection:created", function (e) {
            if (e.selected.length == 1)
                self.itemSelectionChanged(e.selected[0].id);
            else
                self.itemSelectionChanged();
        });
        canvas.on("selection:updated", function (e) {
            if (e.selected.length == 1)
                self.itemSelectionChanged(e.selected[0].id);
            else
                self.itemSelectionChanged();
        });
    }

    /**
     * 캔버스 object:modified 이벤트 핸들러를 추가한다.
     * @param {any} canvas fabric canvas
     */
    addCanvasObjectModifiedHandler(canvas) {
        let self = this;
        canvas.on('object:modified', function (e) {
            let target = e.target;
            if (!target)
                return;

            self.validateItem(target);

            if (target.type == "group") {
                target.refresh();
            }

            canvas.renderAll();
        });
    }

    /**
     * 삭제 키 핸들러를 추가한다.
     * Delete키 입력시 아이템을 삭제한다.
     * @param {any} canvas
     */
    addCanvasDeleteKeyEventHandler(canvas) {
        document.addEventListener("keydown", function (e) {
            if (e.key == "Delete") {
                let item = canvas.getActiveObject();
                if (item) {
                    canvas.remove(item);
                }
            }
        });
    }

    /**
     * 컨트롤 C, V 키 이벤트 핸들러를 추가한다.
     * 복사기능을 제공한다.
     * @param {any} canvas  fabric canvas
     */
    addCanvasCtrlCVKeyEventHandler(canvas) {
        let self = this;
        document.addEventListener("keydown", function (e) {
            // Ctrl+C or Cmd+C pressed?
            if ((event.ctrlKey || event.metaKey) && event.keyCode == 67) {
                // copy
                let item = canvas.getActiveObject();
                if (item)
                    self.copiedItemInfo = item.toItemInfo();
                else
                    self.copiedItemInfo = null;
            }

            // Ctrl+V or Cmd+V pressed?
            if ((event.ctrlKey || event.metaKey) && event.keyCode == 86) {
                // paste
                if (!self.copiedItemInfo)
                    return;
                self.copiedItemInfo.left += self.gridSize * 2;
                self.copiedItemInfo.top += self.gridSize * 2;
                self.copiedItemInfo.itemId = uuidv4();

                const item = self.addItem(self.copiedItemInfo);
                self.canvas.setActiveObject(item);
            }
        });
    }

    /**
     * 캔버스 영역에 드랍 이벤트 발생시 아이템을 추가한다.
     * @param {any} canvas
     */
    addDropEventHandler(canvas) {
        let canvasElement = canvas.getElement();
        let self = this;

        canvasElement.parentElement.addEventListener("drop", function (e) {
            let shape = e.dataTransfer.getData("shape");

            let xPoint = self.snapToGrid(e.layerX / self.zoomLevel);
            let yPoint = self.snapToGrid(e.layerY / self.zoomLevel);
            const item = self.addNewItem(shape, xPoint, yPoint);

            self.canvas.setActiveObject(item);
        });
    }


    snapLeftToGrid(canvasItem) {
        return this.snapToGrid(canvasItem.left);
    }
    snapTopToGrid(canvasItem) {
        return this.snapToGrid(canvasItem.top);
    }

    snapToGrid(value) {
        return Math.round(value / this.gridSize) * this.gridSize
    }

    /**
     * 캔버스 아이템이 유효한 구역에 위치하도록 속성을 수정한다.
     * @param {any} item
     */
    validateItem(item) {
        // scale snap to grid
        item.set({
            scaleX: this.roundScaleX(item),
            scaleY: this.roundScaleY(item)
        });

        // scale validation
        item.set({
            scaleX: this.validateScaleX(item),
            scaleY: this.validateScaleY(item)
        })

        // position snap to grid
        item.set({
            left: this.snapLeftToGrid(item),
            top: this.snapTopToGrid(item)
        });

        // position validation
        item.set({
            left: this.validateLeft(item),
            top: this.validateTop(item)
        });
    }


    // 그리드 아이템 크기에 맞게 반올림한 scaleX값을 반환한다.
    roundScaleX(canvasItem) {
        let scaledWidth = canvasItem.getScaledWidth();
        let remainder = scaledWidth % this.gridSize;
        let roundedWidth = scaledWidth - remainder;
        if ((this.gridSize / 2) <= remainder) {
            roundedWidth += this.gridSize;
        }
        return roundedWidth / canvasItem.width;
    }

    // 그리드 아이템 크기에 맞게 반올림한 scaleY값을 반환한다.
    roundScaleY(canvasItem) {
        let scaledHeight = canvasItem.getScaledHeight();
        let remainder = scaledHeight % this.gridSize;
        let roundedHeight = scaledHeight - remainder;
        if ((this.gridSize / 2) <= remainder) {
            roundedHeight += this.gridSize;
        }
        return roundedHeight / canvasItem.height;
    }

    // 너비 유효성검사. 최소/최대 너비 검사.
    validateScaleX(canvasItem) {
        let scaledWidth = canvasItem.getScaledWidth();

        if (scaledWidth < this.gridSize)
            return this.gridSize / canvasItem.width;
        if (this.canvasWidth < scaledWidth)
            return this.canvasWidth / canvasItem.width;
        else
            return canvasItem.scaleX;
    };

    // 높이 유효성검사. 최소/최대 높이 검사.
    validateScaleY(canvasItem) {
        let scaledHeight = canvasItem.getScaledHeight();
        if (scaledHeight < this.gridSize)
            return this.gridSize / canvasItem.height;
        if (this.canvasHeight < scaledHeight)
            return this.canvasHeight / canvasItem.height;
        else
            return canvasItem.scaleY;
    };

    // left 유효성검사. 오브젝트는 캔버스 안에 위치해야한다.
    validateLeft(canvasItem) {
        let scaledWidth = canvasItem.getScaledWidth();
        if (canvasItem.left < 0)
            return 0;
        else if (this.canvasWidth < canvasItem.left + scaledWidth)
            return this.canvasWidth - scaledWidth;
        else
            return canvasItem.left;
    };

    // top 유효성검사. 오브젝트는 캔버스 안에 위치해야한다.
    validateTop(canvasItem) {
        let scaledHeight = canvasItem.getScaledHeight();
        if (canvasItem.top < 0)
            return 0;
        else if (this.canvasHeight < canvasItem.top + scaledHeight)
            return this.canvasHeight - scaledHeight;
        else
            return canvasItem.top;
    };

    setDefaultOptions(item) {
        item.set({
            lockScalingFlip: true,
            originX: 'left',
            originY: 'top',
            strokeWidth: 0,
            strokeUniform: true,
            borderColor: this.colors.border,
            cornerColor: this.colors.corner,
            cornerStrokeColor: this.colors.cornerStroke,
            transparentCorners: false,
        });
        item.setControlVisible("mtr", false);
    }

    // 캔버스 아이템을 모두 삭제합니다.
    clear() {
        let groups = this.canvas.getObjects("group");
        for (const group of groups) {
            this.canvas.remove(group);
        }
    }

    dispose() {
        this.canvas.dispose();
    }

    getImageUrlById(imageId) {
        return this.getImage(imageId);
    }

    // 팔레트 아이템으로 지정한다. 
    // elementId: 요소 아이디
    // shape: 오브젝트 모양("rect", "circle")
    setPaletteItem(elementId, shape, imageSrc) {
        let imgElement = document.getElementById(elementId);

        if (!imageSrc) {
            if (shape == "rect")
                imgElement.src = this.createRectImageURL();
            else
                imgElement.src = this.createCircleImageURL();
        }

        imgElement.addEventListener("dragstart", function dragStart(e) {
            e.dataTransfer.setDragImage(imgElement, 0, 0);
            e.dataTransfer.setData("shape", shape);
        });
    }


    createRectImageURL() {
        const canvas = document.createElement("canvas")
        canvas.width = this.itemSize;
        canvas.height = this.itemSize;

        const ctx = canvas.getContext("2d");
        ctx.fillStyle = this.colors.fill;
        ctx.fillRect(0, 0, canvas.width, canvas.height)

        return canvas.toDataURL();
    }

    createCircleImageURL() {
        const canvas = document.createElement("canvas")
        canvas.width = this.itemSize;
        canvas.height = this.itemSize;

        const ctx = canvas.getContext("2d");
        const radius = this.itemSize / 2;
        ctx.arc(radius, radius, radius, 0, 2 * Math.PI);
        ctx.fillStyle = this.colors.fill;
        ctx.fill();

        return canvas.toDataURL();
    }

    // 컬러인풋으로 지정한다.
    // 컬러변경시 현재 활성화된 오브젝트의 배경을 변경한다.
    // colorInput: 컬러 인풋
    setColorInput(colorInput) {
        let self = this;
        colorInput.addEventListener("change", function (e) {
            let item = self.canvas.getActiveObject();
            if (!item)
                return;

            self.setItemBackColor(item, this.value);
        });
    }

    // 파일인풋으로 지정한다.
    // 파일변경시 현재 활성화된 오브젝트의 배경을 변경한다.
    // fileInput: 파일 인풋
    setFileInput(fileInput) {
        let self = this;
        fileInput.addEventListener("change", function (e) {
            let item = self.canvas.getActiveObject();
            if (!item)
                return;

            var reader = new FileReader();
            reader.onload = function () {

                let arrayBuffer = this.result;
                let array = new Uint8Array(arrayBuffer);

                const blob = new Blob([array], { type: "image/jpeg" });
                let imageUrl = URL.createObjectURL(blob);
                let imageId = "a";
                self.setCanvasItemImage(item, imageUrl, imageId);
            }

            reader.readAsArrayBuffer(this.files[0]);
        });
    }

    // 이미지 불러오기 함수를 설정한다.
    setImageFunc(func) {
        this.getImage = func;
    }


    // 캔버스 아이템의 배경 이미지를 변경한다. 아이템이 그룹인 경우 shape 아이템의 배경 이미지를 변경한다.
    // canvasItem: 변경할 캔버스 아이템
    // imageUrl: 이미지 url 주소
    setCanvasItemImage(canvasItem, imageUrl, imageId) {
        let self = this;
        let shape = canvasItem;
        if (canvasItem instanceof fabric.Group) {
            shape = canvasItem.item(0);
        }
        fabric.Image.fromURL(imageUrl, function (img, isError) {
            if (isError) {
                console.log("faile to load image");
                return;
            }
            // 캔버스 아이템 배경 이미지 생성
            img.scaleX = shape.width / img.width;
            img.scaleY = shape.height / img.height;

            let patternCanvas = new fabric.StaticCanvas();
            patternCanvas.setDimensions({
                width: img.getScaledWidth(),
                height: img.getScaledHeight()
            });
            patternCanvas.add(img);
            patternCanvas.renderAll();

            // 캔버스 아이템 배경 이미지 설정
            var pattern = new fabric.Pattern({
                source: patternCanvas.getElement(),
                repeat: 'no-repeat',
                imageId: imageId,
                crossOrigin: "anonymous"
            });

            shape.set({
                fill: pattern
            });
            self.canvas.renderAll();
        }, {
            // cors정책으로 캔버스에 이미지가 포함되었을 경우 직렬화에 문제가 발생한다.
            crossOrigin: "anonymous"
        });
    }


    /** public methods **/

    /**
    * 그리드 라인을 그린다.
    * 캔버스 너비/높이가 그리드 크기의 정수배가 되도록 한다.
    */
    drawGridLines() {
        // 캔버스 경계를 제외하고 안쪽에만 라인을 그린다.
        let hLineCount = (this.canvas.width / this.gridSize) - 1;
        let vLineCount = (this.canvas.height / this.gridSize) - 1;

        let lineOptions = { stroke: '#ccc', selectable: false, hoverCursor: "default" };

        for (let i = 1; i <= hLineCount; i++) {
            let verticalLine = new fabric.Line([i * this.gridSize, 0, i * this.gridSize, this.canvas.height], lineOptions);
            this.canvas.add(verticalLine);
        }
        for (let i = 1; i <= vLineCount; i++) {
            let horizontalLine = new fabric.Line([0, i * this.gridSize, this.canvas.width, i * this.gridSize], lineOptions);
            this.canvas.add(horizontalLine);
        }
    }

    zoom(level) {
        let nextWidth = this.canvasWidth * level;
        let nextHeight = this.canvasHeight * level;

        this.canvas.setWidth(nextWidth);
        this.canvas.setHeight(nextHeight);

        this.canvas.setZoom(level);

        this.zoomLevel = level;
    }

    deleteItem(item) {
        this.canvas.remove(item);

        let shape = item;
        if (item instanceof fabric.Group) {
            shape = item.item(0);
        }

        // 이미지 캔버스 삭제
        delete this.sourceCanvasElementDict[shape];
    }


    /**
     * 신규 아이템을 캔버스에 추가한다.
     * @param {any} options
     */
    addNewItem(shape, left, top) {
        let itemProperties = new ItemInfo();
        itemProperties.itemId = uuidv4();
        itemProperties.shape = shape;
        itemProperties.left = left;
        itemProperties.top = top;
        itemProperties.width = this.itemSize;
        itemProperties.height = this.itemSize;

        return this.addItem(itemProperties);
    }

    /**
     * 아이템을 캔버스에 추가한다.
     * @param {ItemInfo} itemInfo 아이템 옵션
     */
    addItem(itemInfo) {
        let group = new fabric.Group();
        this.setDefaultOptions(group);

        group.id = itemInfo.itemId;
        group.shape = itemInfo.shape;

        let shapeItem;
        if (itemInfo.shape == "rect") {
            shapeItem = new fabric.Rect({
                width: itemInfo.width * 100,
                height: itemInfo.height * 100,
                scaleX: 0.01,
                scaleY: 0.01
            });
        }
        else {
            let radius = Math.max(itemInfo.width, itemInfo.height) / 2 * 100;
            shapeItem = new fabric.Circle({
                radius: radius,
                scaleX: itemInfo.width / (2 * radius),
                scaleY: itemInfo.height / (2 * radius),
            });
        }
        this.setDefaultOptions(shapeItem);
        shapeItem.stroke = "black";
        shapeItem.strokeWidth = 1;

        let textItem = new fabric.Text('');
        this.setDefaultOptions(textItem);

        group.addWithUpdate(shapeItem);
        group.addWithUpdate(textItem);
        group.set({
            left: itemInfo.left,
            top: itemInfo.top
        });

        // 텍스트 변경과정에서 아이템의 크기가 변할 수 있기 때문에 텍스트 각도를 미리 설정해야한다.
        // 세로텍스트를 가로텍스트로 인식하는 경우 너비가 변경되고 반대의 경우 높이가 변경된다.
        group.setDegree(itemInfo.degree);
        group.setText(itemInfo.text);
        group.setFontSize(itemInfo.fontSize);
        group.setHAlignment(itemInfo.hAlignment);
        group.setVAlignment(itemInfo.vAlignment);
        group.setBackColor(itemInfo.backColor);

        this.validateItem(group);
        this.canvas.add(group);

        return group;
    }

    getItemInfo(id) {
        let groupItem = this.canvas.getItemById(id);
        if (!groupItem)
            return null;
        let shapeItem = groupItem.item(0);

        let isPattern = (shapeItem.fill instanceof fabric.Pattern);
        let backColor = null;
        let patternImageId = null;
        if (isPattern)
            patternImageId = shapeItem.fill.imageId;
        else
            backColor = shapeItem.fill;

        let itemInfo = new ItemInfo();
        itemInfo.itemId = groupItem.id;
        itemInfo.isPattern= isPattern;
        itemInfo.backColor= backColor;
        itemInfo.patternImageId= patternImageId;
            
        itemInfo.text= groupItem.text;
        itemInfo.fontSize= groupItem.fontSize;
        itemInfo.vAlignment= groupItem.vAlignment;
        itemInfo.hAlignment= groupItem.hAlignment;
        itemInfo.degree= groupItem.degree

        return itemInfo;
    }

    getItemById(id) {
        return this.canvas.getItemById(id);
    }


    /**
     * 캔버스의 모든 아이템을 반환한다.
     * */
    getAllItems() {
        return this.canvas.getObjects("group");
    }

    // 캔버스 아이템의 배경색을 변경한다. 아이템이 그룹인 경우 shape 아이템의 배경색을 변경한다.
    // canvasItem: 변경할 캔버스 아이템
    // color: 변경색상 rgb코드
    setItemBackColor(groupItem, color) {
        groupItem.setBackColor(color);
        this.canvas.renderAll();
    }

    setItemText(groupItem, text) {
        groupItem.setText(text);
        this.canvas.renderAll();
    }

    setItemFontSize(groupItem, fontSize) {
        groupItem.setFontSize(fontSize);
        this.canvas.renderAll();
    }

    setItemHAlignment(groupItem, hAlignment) {
        groupItem.setHAlignment(hAlignment);
        this.canvas.renderAll();
    }

    setItemVAlignment(groupItem, vAlignment) {
        groupItem.setVAlignment(vAlignment);
        this.canvas.renderAll();
    }

    setItemDegree(groupItem, degree) {
        groupItem.setDegree(degree);
        this.canvas.renderAll();
    }

    exportItemInfos() {
        let groups = this.getAllItems();
        let itemInfos = new Array();
        for (const group of groups) {
            itemInfos.push(group.toItemInfo());
        }
        return itemInfos;
    }

    importItemInfos(itemInfos) {
        for (const itemInfo of itemInfos) {
            this.addItem(itemInfo);
        }
    }

    /**
     * 캔버스의 상호작용 기능을 활성화/비활성화 한다.
     * @param {any} enabled 상호작용 활성화 여부
     */
    setInteraction(enabled) {
        this.canvas.hoverCursor = enabled ? "move" : "default";
        let items = this.getAllItems();
        for (const item of items) {
            item.selectable = enabled;
        }
    }

}
