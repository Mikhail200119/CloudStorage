function handleFileDoubleClick(fileSrc, contentType) {
    const overlayElement = document.getElementById("myOverlay");
    overlayElement.style.display = "block";

    let file = null;    

    if (contentType === "image/png") {
        file = document.createElement("img");
    }
    else if (contentType === "video/mp4") {
        file = document.createElement("video");
        file.id = "videoplayer";
        file.controls = true;
    }

    if (file === null) {
        return;
    }

    file.height = "550";
    file.width = "550";
    file.src = fileSrc;
    file.style.alignSelf = "center";

    const closeButton = document.createElement("button");
    closeButton.textContent = "Close";
    closeButton.style = "height: 100px; width: 200px";
    closeButton.onclick = closeFileOverlay;

    overlayElement.alignItems = "center";

    overlayElement.appendChild(file);
    overlayElement.appendChild(document.createElement("br"));
    overlayElement.appendChild(closeButton);
}

function closeFileOverlay() {
    const overlay = document.getElementById("myOverlay");
    overlay.style.display = "none";
    overlay.innerHTML = "";
}