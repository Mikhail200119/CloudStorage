// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addNewFile(parameterName, controllerActionUrl) {
    createNewFolder(parameterName, controllerActionUrl);
}

function createNewFolder(parameterName, controllerActionUrl) {
    const currentFolderId = document.getElementById("currentFolderId").textContent;

    window.location.href = `${controllerActionUrl}/${parameterName}=${currentFolderId}`;
}