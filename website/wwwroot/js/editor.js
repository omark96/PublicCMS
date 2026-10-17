function createLink(){
    let url = prompt('Enter a URL:', 'https://');
    document.execCommand('createlink', false, url);
}