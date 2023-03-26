function handleFileDoubleClick(fileSrc, contentType) {
    const overlayElement = document.getElementById("myOverlay");
    overlayElement.style.display = "block";

    if(contentType === "application/octet-stream") {
        contentType = "text/plain";
    }
    
    let file = null;

    if (isImage(contentType)) {
        file = document.createElement("img");
        file.setAttribute("src", `${fileSrc}`);
    } else if (isVideo(contentType)) {
        file = document.createElement("video");
        file.setAttribute("class", "file-video-player");
        file.setAttribute("controls", "");
        file.setAttribute("src", `${fileSrc}`);
    } else if (isText(contentType)) {
        file = document.createElement("iframe");
        file.setAttribute("type", `${contentType}`);
        file.style = "height: 700px; width: 1000px; object-fit: contain; background-color: white;";
        file.setAttribute("src", `${fileSrc}`);
    }

    if (file === null) {
        closeFileOverlay();
        return;
    }

    const closeButton = document.createElement("button");
    closeButton.textContent = "X";
    closeButton.id = "overlayCloseItem";
    closeButton.style = "height: 50px; width: 50px";
    closeButton.onclick = closeFileOverlay;

    const divElement = document.createElement("div");
    divElement.id = "overlayFile";

    divElement.appendChild(file);
    divElement.appendChild(document.createElement("br"));

    overlayElement.appendChild(divElement);
    overlayElement.appendChild(closeButton);
}

function closeFileOverlay() {
    const overlay = document.getElementById("myOverlay");
    overlay.style.display = "none";
    overlay.innerHTML = "";

    document.getElementById("office-documents-viewer").style.display = "none";
}