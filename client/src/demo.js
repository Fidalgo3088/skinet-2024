var message = "Hello";
// let message = "Hello";
var isComplete = false;
// let isComplete = false;
message = 1;
var todos = [];
function addTodo(title) {
    var newTodo = {
        id: todos.length + 1,
        title: title,
        completed: false
    };
    todos.push(newTodo);
    return newTodo;
}
function toggleTodo(id) {
    var todo = todos.find(function (todo) { return todo.id == id; });
    if (todo) {
        todo.completed = !todo.completed;
    }
    addTodo("Build API");
    addTodo("Publish app");
    addTodo(1);
    toggleTodo(1);
}
