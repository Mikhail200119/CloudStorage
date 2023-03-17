const imageContentTypes = [ "image/png", "image/jpeg", "image/jpg" ];
const videoContentTypes = [ "video/mp4" ];
const textContentTypes = [ "text/plain", "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ];

function isImage(contentType) {
   return imageContentTypes.includes(contentType);
}

function isVideo(contentType){
    return videoContentTypes.includes(contentType);
}

function isText(contentType){
    return textContentTypes.includes(contentType);
}