class Localizer {
    static login = (lan) => {
        if (lan === 'ru') return 'Войти';
        else if (lan === 'et') return 'Sisu';
        else return 'Login';
    }
    static email = (lan) => {
        if (lan === 'ru') return 'Email:';
        else if (lan === 'et') return 'Email:';
        else return 'Email:';
    }
    static password = (lan) => {
        if (lan === 'ru') return 'Пароль:';
        else if (lan === 'et') return 'Parolli:';
        else return 'Password:';
    }
    static wrongPassword = (lan) => {
        if (lan === 'ru') return 'Неверный email или пароль!';
        else if (lan === 'et') return 'Viga parol';
        else return 'Wrong email or password!';
    }
    static connectionError = (lan) => {
        if (lan === 'ru') return 'Невозможно подключиться к серверу. Проверьте подключение к интернету.';
        else if (lan === 'et') return 'Ei oli interneti';
        else return 'Failed to connect to server. Check internet connection.';
    }
    static emptyPassword = (lan) => {
        if (lan === 'ru') return 'Email и пароль не могут быть пустыми!';
        else if (lan === 'et') return 'Tuhi parol';
        else return 'Email and password can not be empty!';
    }
    static main = (lan) => {
        if (lan === 'ru') return 'Главная';
        else if (lan === 'et') return 'Main';
        else return 'Main';
    }
    static favorites = (lan) => {
        if (lan === 'ru') return 'Избранное';
        else if (lan === 'et') return 'Lemmik';
        else return 'Favorites';
    }
    static settings = (lan) => {
        if (lan === 'ru') return 'Настройки';
        else if (lan === 'et') return 'Settings';
        else return 'Settings';
    }
    static search = (lan) => {
        if (lan === 'ru') return 'Искать';
        else if (lan === 'et') return 'Otsi';
        else return 'Search';
    }
}