var OpenWindowPlugin = {
    copyClipboard: function(textPointer) {
        try {
            const encodedText = UTF8ToString(textPointer);
            console.log(encodedText);
            const byteString = atob(encodedText);
            console.log(byteString);
            const ab = new ArrayBuffer(byteString.length);
            const ia = new Uint8Array(ab);
            for (let i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            const pngBlob = new Blob([ab], {type: 'image/png'});
            console.log(pngBlob);
            navigator.clipboard.write([
                new ClipboardItem({
                    'image/png': pngBlob
                })
            ]);
        } catch (error) {
            console.error(error);
        }
    }
};

mergeInto(LibraryManager.library, OpenWindowPlugin);
