# LibraryAPI

## Описание
Этот проект представляет собой API для управления библиотекой, созданный с использованием Clean Architecture.

## Структура проекта
- **Domain**: Содержит доменные сущности и интерфейсы.
- **Application**: Включает логику приложения и сервисы.
- **Infrastructure**: Реализация доступа к данным и внешним сервисам.
- **WebAPI**: Слой представления с контроллерами и middleware.

## Commands
- Use
'dotnet ef migrations add InitialCreate --project LibraryAPI.Infrastructure --startup-project LibraryAPI.WebAPI'
in terminal from the directory _**..\LibraryAPI\server\src**_ for add migration

- Use
'dotnet ef database update --project LibraryAPI.Infrastructure --startup-project LibraryAPI.WebAPI'
in terminal from the directory _**..\LibraryAPI\server\src**_ for update database