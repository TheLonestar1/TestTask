# Приложение по учёту сотрудников
## Требования к запуску:
 1)SQLite
 2).Net
 ## Запуск:
 Собранный проект хранится в папке Realse, и запускается через TestTask.exe
 ## Пояснительная записка:
 
 Для начала работы стоит пройтись по интерфейсу
 
![Снимок экрана (193)](https://user-images.githubusercontent.com/43960228/170878111-8a477307-7f65-4995-8414-5ca447efd6dd.png)

Где:
Таблица с данными о сотрудниках

Кнопки: 

"Добавить сотрудника" где после нажатия на эту кнопку открывается окно для ввода данных о новом сотруднике

![Снимок экрана (195)](https://user-images.githubusercontent.com/43960228/170878127-bb07610c-24d7-450b-8e13-a6c50cf7dabe.png)

Где указывается имя, дата рождения, пол. Подразделения этого сотрудника ,и если этим подразделением никто не руководит, то выведется окно с сообщением об ошибке ввода
,но если выбрана должность руководителя подразделения ,то создаётся новое подразделение. Далее идёт выбор должности из выподающего списка

После идёт кнопка удаления выделенной строчки 

Потом следует сегмент выборки сотрудников по данным о подзразделении и должности которые выбираются в соответсвующих полях
