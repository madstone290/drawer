let drawer;

export function initDrawer(canvasMediatorRef, canvasId, paletteItems, useInterval) {
    console.log("initDrawer", canvasMediatorRef, canvasId, paletteItems, useInterval);
    drawer = new Drawer(canvasId, useInterval);

    for (const item of paletteItems) {
        drawer.setPaletteItem(item.elementId,
            item.shape,
            item.imageSrc);
    }

    drawer.itemSelectionChanged = function (id) {
        canvasMediatorRef.invokeMethodAsync('ItemSelectionChanged', id);
    };
}

export function drawGridLines() {
    console.log("drawGridLines");
    drawer.drawGridLines();
}

export function zoom(level) {
    console.log("zoom", level);
    drawer.zoom(level);
}

export function getItemInfo(id) {
    console.log("getItemInfo", id);
    return drawer.getItemInfo(id);
}

export function setBackColor(id, colorCode) {
    console.log("setBackColor", id, colorCode);
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemBackColor(item, colorCode);
}

export function setText(id, text) {
    console.log("setText", id, text);
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemText(item, text);
}

export function setFontSize(id, fontSize) {
    console.log("setFontSize", id, fontSize);
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemFontSize(item, fontSize);
}

export function setHAlignment(id, hAlignment) {
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemHAlignment(item, hAlignment);
}

export function setVAlignment(id, vAlignment) {
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemVAlignment(item, vAlignment);
}

export function setDegree(id, degree) {
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemDegree(item, degree);
}

export function deleteItem(id) {
    console.log("deleteItem", id);
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.deleteItem(item);
}

export function clearCanvas() {
    console.log("clearCanvas");
    drawer.clear();
}

export function exportItemList() {
    console.log("exportItemList");
    return drawer.exportItemInfos();
}

export function importItemList(itemList) {
    console.log("importItemList", itemList);
    drawer.importItemInfos(itemList);
}

export function setInteraction(enabled) {
    console.log("setInteraction", enabled);
    drawer.setInteraction(enabled);
}

export function setBlink(itemIdList) {
    console.log("setBlink", itemIdList);
    let itemList = new Array();
    for (const id of itemIdList) {
        itemList.push(drawer.getItemById(id));
    }
    drawer.setBlink(itemList);
}

export function dispose() {
    console.log("dispose");
    drawer.dispose();
    drawer = null;
}
