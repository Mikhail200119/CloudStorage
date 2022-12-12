// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addNewFile(parameterName, controllerActionUrl) {
    const currentFolderName = document
        .querySelectorAll("div[id=textInFolderOfLine]")
        .map(text => text.nodeValue)
        .pop();

    window.location.href = `${controllerActionUrl}/${parameterName}=${currentFolderName}`;
}