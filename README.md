# BoomTestTask

## Описание

Это API для управления кошельками пользователей, их балансом и транзакциями.

## Предварительные требования

*   [Docker](https://www.docker.com/get-started) (Установите Docker Desktop для вашей операционной системы).

## Инструкции по запуску

1.  **Клонируйте репозиторий:**

    ```bash
    git clone <ваш_репозиторий_URL>
    cd <название_вашего_репозитория>
    ```

2.  **Сборка Docker образа:**

    Убедитесь, что в корневой директории вашего проекта есть `Dockerfile`. Если его нет, то его нужно создать (см. пример ниже).
    Запустите команду сборки Docker образа:

    ```bash
    docker build -t wallet-api .
    ```

    Здесь:
    *   `wallet-api` - имя вашего Docker образа. Вы можете его изменить.
    *   `.` - указывает, что Dockerfile находится в текущей директории.

3.  **Запуск Docker контейнера:**

    Запустите контейнер из созданного образа, пробросив порт 80 (или другой, указанный в `Dockerfile`) на порт, который вы хотите использовать на вашей локальной машине.

    ```bash
    docker run -p 32771:80 wallet-api
    ```

    Здесь:
    *   `32771` - порт на вашей локальной машине, на котором будет доступно приложение. (Убедитесь, что порт не занят другим приложением.)
    *   `wallet-api` - имя Docker образа, которое вы указали на шаге 2.
    *   `80` - порт, который приложение слушает внутри Docker контейнера.

4.  **Доступ к API:**

    После запуска контейнера API будет доступен по адресу:

    ```
    https://localhost:32771/swagger/index.html
    ```

    Откройте этот URL в вашем браузере, чтобы увидеть Swagger UI.

## Пример Dockerfile

Если у вас еще нет `Dockerfile`, вот пример, который можно использовать. Создайте файл с именем `Dockerfile` в корневой директории проекта и добавьте следующее содержимое:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["<название_вашего_проекта>.csproj", "./"]
RUN dotnet restore "./<название_вашего_проекта>.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "<название_вашего_проекта>.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "<название_вашего_проекта>.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "<название_вашего_проекта>.dll"]
