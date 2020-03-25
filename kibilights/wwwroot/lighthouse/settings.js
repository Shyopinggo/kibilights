let accessToken = window.localStorage.getItem('access_token');
let userName = window.localStorage.getItem('user_name');
if (!accessToken || !userName) document.location = 'index.html';

let lang = window.localStorage.getItem('language');
if (!lang) lang = 'en';
$('html').attr('lang', lang);

function parseMultiLangContent(content) {
    let result = null;
    try {
        result = JSON.parse(content);
    }
    catch (err) { }
    if (result) result = result[lang];
    if (!result) result = content;
    return result;
}