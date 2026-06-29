const API_URL = '/todos';

async function loadTodos() {
    const response = await fetch(API_URL);
    const todos = await response.json();
    renderTodos(todos);
}

function renderTodos(todos) {
    const list = document.getElementById('todoList');
    list.innerHTML = '';

    todos.forEach(todo => {
        const li = document.createElement('li');
        li.textContent = todo.title;
        list.appendChild(li);
    });
}

loadTodos();

document.getElementById('createForm').addEventListener('submit', async (event) => {
    event.preventDefault(); // отменяем стандартное поведение формы (перезагрузку страницы)

    const title = document.getElementById('titleInput').value;
    const description = document.getElementById('descriptionInput').value;

    await fetch(API_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ title, description })
    });

    document.getElementById('titleInput').value = '';
    document.getElementById('descriptionInput').value = '';

    loadTodos(); // перезагружаем список после создания
});