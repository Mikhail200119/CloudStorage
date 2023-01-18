function onFileRename(element) {
    const actionPath = document.getElementById("rename-file-url").textContent;
    console.log(actionPath);
    const fileId = element.querySelector("div[id=fileId]").textContent;
    displayRenameInput(actionPath, fileId);
}

function displayRenameInput(actionPath, fileId) {
    const overlay = document.getElementById("myOverlay");
    overlay.style.display = "block";
    const input = document.createElement("input");
    input.type = "text";
    const renameButton = document.createElement("button");
    renameButton.textContent = "Rename";
    renameButton.addEventListener("click", event => {
        const newName = input.value;
        window.location.href = `${actionPath}?id=${fileId}&newName=${newName}`;
    });
    
    const cancelButton = document.createElement("button");
    cancelButton.textContent = "Cancel";
    cancelButton.addEventListener("click", event => {
        overlay.style.display = "none";
        overlay.innerHTML = "";
    });
    
    overlay.append(input, renameButton, cancelButton);
}