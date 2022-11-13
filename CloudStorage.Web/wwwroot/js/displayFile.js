﻿function handleFileDoubleClick(fileSrc, contentType) {
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
    else if (contentType === "text/plain") {
        file = document.createElement("iframe");
    }

    if (file === null) {
        return;
    }

    file.height = "550";
    file.width = "550";
    file.src = fileSrc;

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
}