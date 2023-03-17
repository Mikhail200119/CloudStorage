function onFileRename() {
    const actionPath = document.getElementById("rename-file-url").textContent;
    const fileId = document
        .querySelector("div[id=context-menu]")
        .querySelector("div[id=fileId]").textContent;
    
    displayRenameInput(actionPath, fileId);
}

function displayRenameInput(actionPath, fileId) {
    const overlay = document.getElementById("myOverlay");
    overlay.style.display = "block";
    const input = document.createElement("input");
    input.type = "text";
    input.placeholder = "Type new name...";
    const renameButton = document.createElement("button");
    renameButton.textContent = "Rename";
    renameButton.className = "rename-button";
    renameButton.addEventListener("click", async () => {
        const newName = input.value;
        const url = `${actionPath}?id=${fileId}&newName=${newName}`;
        await fetch(url, { method: 'PUT' })
            .then(response => {
                if(response.ok) {
                    renameFileInPage(fileId, newName);
                    closeFileOverlay();
                }
            })
            .catch(() => console.log("An error during renaming file occured."));
    });
    
    const cancelButton = document.createElement("button");
    cancelButton.textContent = "Cancel";
    cancelButton.className = "cancel-button"
    cancelButton.addEventListener("click", event => {
        closeFileOverlay();
    });
    
    const container = document.createElement("div");
    container.className = "rename-file-container";
    container.append(input, renameButton, cancelButton);
    
    overlay.appendChild(container);
}

async function onFileDelete() {
    const actionPath = document.getElementById("delete-file-url").textContent;
    const fileId = document
        .querySelector("div[id=context-menu]")
        .querySelector("div[id=fileId]").textContent;

    await fetch(`${actionPath}?id=${fileId}`, { method: 'DELETE' })
        .then(response => {
            if (response.ok) {
                removeFileItemOnPage(fileId);
            }
        })
        .catch(() => console.log("An error during deleting file occured"));
}

function renameFileInPage(fileId, newName) {
    const fileItemToRename = getFileByIdFromPage(fileId);
    fileItemToRename.querySelector("div[id=fileName]").textContent = newName;
}

function getFileByIdFromPage(fileId) {
    return Array.from(document.querySelectorAll("div[class=fileItemBox]"))
        .filter(item => item.querySelector("div[id=fileId]").textContent === fileId)[0];
}