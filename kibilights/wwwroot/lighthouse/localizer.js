class Localizer {
    static login(lan) {
        if (lan === 'ru') return 'Войти';
        else if (lan === 'et') return 'Sisu';
        else return 'Login';
    }
    static email(lan) {
        if (lan === 'ru') return 'Email:';
        else if (lan === 'et') return 'Email:';
        else return 'Email:';
    }
    static password(lan) {
        if (lan === 'ru') return 'Пароль:';
        else if (lan === 'et') return 'Parolli:';
        else return 'Password:';
    }
    static wrongPassword(lan) {
        if (lan === 'ru') return 'Неверный email или пароль!';
        else if (lan === 'et') return 'Viga parol';
        else return 'Wrong email or password!';
    }
    static connectionError(lan) {
        if (lan === 'ru') return 'Невозможно подключиться к серверу. Проверьте подключение к интернету.';
        else if (lan === 'et') return 'Ei oli interneti';
        else return 'Failed to connect to server. Check internet connection.';
    }
    static emptyPassword(lan) {
        if (lan === 'ru') return 'Email и пароль не могут быть пустыми!';
        else if (lan === 'et') return 'Tuhi parol';
        else return 'Email and password can not be empty!';
    }
    static main(lan) {
        if (lan === 'ru') return 'Главная';
        else if (lan === 'et') return 'Main';
        else return 'Main';
    }
    static favorites(lan) {
        if (lan === 'ru') return 'Избранное';
        else if (lan === 'et') return 'Lemmik';
        else return 'Favorites';
    }
    static settings(lan) {
        if (lan === 'ru') return 'Настройки';
        else if (lan === 'et') return 'Settings';
        else return 'Settings';
    }
    static search(lan) {
        if (lan === 'ru') return 'Искать';
        else if (lan === 'et') return 'Otsi';
        else return 'Search';
    }
    static logout(lan) {
        if (lan === 'ru') return 'Выход';
        else if (lan === 'et') return 'Valja';
        else return 'Logout';
    }
    static language(lan) {
        if (lan === 'ru') return 'Язык';
        else if (lan === 'et') return 'Keel';
        else return 'Language';
    }
    static country(lan) {
        if (lan === 'ru') return 'Страна';
        else if (lan === 'et') return 'Riig';
        else return 'Country';
    }
    static city(lan) {
        if (lan === 'ru') return 'Город';
        else if (lan === 'et') return 'Linn';
        else return 'City';
    }
    static save(lan) {
        if (lan === 'ru') return 'Сохранить';
        else if (lan === 'et') return 'Save';
        else return 'Save';
    }
    static addToFavorites(lan) {
        if (lan === 'ru') return 'В избранное';
        else if (lan === 'et') return 'Lemmik';
        else return 'Add to favorites';
    }
    static deleteFromFavorites(lan) {
        if (lan === 'ru') return 'Удалить из избранного';
        else if (lan === 'et') return 'Lemmik valja';
        else return 'Delete from favorites';
    }
}