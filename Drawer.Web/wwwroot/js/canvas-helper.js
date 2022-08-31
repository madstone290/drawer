let drawer;

export function initDrawer(canvasMediatorRef, canvasId, paletteItems, useInterval) {
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
    drawer.drawGridLines();
}

export function zoom(level) {
    drawer.zoom(level);
}

export function getItemInfo(id) {
    return drawer.getItemInfo(id);
}

export function setBackColor(id, colorCode) {
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemBackColor(item, colorCode);
}

export function setText(id, text) {
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.setItemText(item, text);
}

export function setFontSize(id, fontSize) {
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
    let item = drawer.getItemById(id);
    if (!item)
        return;
    drawer.deleteItem(item);
}

export function clearCanvas() {
    drawer.clear();
}

export function exportItemList() {
    return drawer.exportItemInfos();
}

export function importItemList(itemList) {
    drawer.importItemInfos(itemList);
}

export function setInteraction(enabled) {
    drawer.setInteraction(enabled);
}

export function setBlink(itemIdList) {
    let itemList = new Array();
    for (const id of itemIdList) {
        console.log(id);
        itemList.push(drawer.getItemById(id));
    }
    drawer.setBlink(itemList);
}

export function dispose() {
    drawer.dispose();
    drawer = null;
}
