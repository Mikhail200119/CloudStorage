function handleFileDoubleClick(fileSrc, contentType) {
    const overlayElement = document.getElementById("myOverlay");
    overlayElement.style.display = "block";
    
    let file = null;
    alert("start");
    if (isImage(contentType)) {
        file = document.createElement("img");
    } else if (isVideo(contentType)) {
        file = document.createElement("video");
        file.setAttribute("class", "file-video-player");
        file.setAttribute("controls", "");
    } else if (isText(contentType)) {
        file = document.createElement("iframe");
    }

    if (file === null) {
        return;
    }

    file.src = fileSrc;
    file.setAttribute("src", `${fileSrc}`);

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
    const videoElement = overlay.querySelector("div[id=overlayFile] video");
    
    if (videoElement !== null) {
        videoElement.pause();
        videoElement.removeAttribute("src");
        videoElement.load();
    }
    
    overlay.style.display = "none";
    overlay.innerHTML = "";
}