function onFileEnter(element) {
    element.querySelector("div[id=selector]").className = "fileSelected";
}

function onFileLeave(element) {
    element.querySelector("div[id=selector]").className = "";
}