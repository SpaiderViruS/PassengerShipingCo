# PassengerShippingCo

🎓 Курсовой проект на C# / WPF, посвящённый системе пассажирского судоходства.  
Позволяет пользователям бронировать круизы, капитанам — управлять судами, а администраторам — контролировать данные системы.

---

## 📋 Описание

PassengerShippingCo реализует следующую функциональность:

- **Пассажир**:
  - Просмотр доступных туров.
  - Бронирование мест на судне.

- **Капитан**:
  - Просмотр свободных судов.
  - Назначение себя на туры.

- **Администратор**:
  - Управление пользователями, капитанами, судами и турами.

---

## 🛠 Технологии

- **Язык**: C#
- **Платформа**: .NET / WPF
- **ORM**: Entity Framework
- **База данных**: Microsoft SQL Server

---

## 📁 Структура проекта

```text
PassengerShipingCo/
├── Entity/           # Модели Entity Framework
├── Images/           # Графические ресурсы
├── Properties/       # Настройки приложения
├── UserControls/     # Компоненты UI (WPF UserControls)
├── Windows/          # Окна интерфейса (главные формы)
├── App.xaml          # WPF точка входа
├── App.config        # Конфигурация подключения к БД
├── packages.config   # Зависимости NuGet
└── PassengerShipingCo.sln  # Visual Studio решение
```

---

## ⚙️ Установка и запуск

1. Убедитесь, что установлено:
   - Visual Studio (рекомендуется 2019+)
   - .NET Framework 4.x
   - Microsoft SQL Server (Express / LocalDB)

2. Клонируйте репозиторий:
```bash
git clone https://github.com/SpaiderViruS/PassengerShipingCo.git
```

3. Откройте решение `.sln` в Visual Studio.

4. В `App.config` настройте строку подключения:
```xml
<connectionStrings>
  <add name="ShippingDB" connectionString="Data Source=.;Initial Catalog=PassengerShippingDB;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

5. Создайте базу данных `PassengerShippingDB` вручную или с помощью миграций EF.

6. Запустите проект (`F5`).

---

## 🧪 Демо-учётки (по желанию)

| Роль        | Логин   | Пароль    |
|-------------|---------|-----------|
| Админ       | admin   | admin123  |
| Капитан     | captain | captain123|
| Пассажир    | user    | user123   |

---

## 📝 Лицензия

Курсовой проект — использование свободное. Авторство: SpaiderViruS.
