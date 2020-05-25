var defaultStart = "\\documentclass{ article }\n\n\\addtolength{ \\oddsidemargin } { -100in}\n\\addtolength{ \\evensidemargin } { -100in}\n\\begin{ document }";
var defaultEnd = "\n\\end{document}";

function OnLoad() {
    var url = new URL(document.URL);
    var search_params = url.searchParams;

    if (search_params.has('selectbasic'))
        document.getElementById('selectbasic').value = search_params.get('selectbasic');
    if (search_params.has('textinput1'))
        document.getElementById('textinput1').value = search_params.get('textinput1');
    if (search_params.has('textinput2'))
        document.getElementById('textinput2').value = search_params.get('textinput2');
    if (search_params.has('textinput3'))
        document.getElementById('textinput3').value = search_params.get('textinput3');
    
    if (Check1() & Check2() & Check3()) {
        document.title = "Генератор задач   Параграф: " + document.getElementById('selectbasic').value +
            ", Вариантов: " + document.getElementById('textinput1').value +
            ", Заданий: " + document.getElementById('textinput2').value +
            ", Ключ: " + document.getElementById('textinput3').value;
    }
}

function Clear() {
    document.getElementById('textinput1').classList.remove("is-invalid");
    document.getElementById('textinput1').classList.remove("is-valid");
    document.getElementById('textinput2').classList.remove("is-invalid");
    document.getElementById('textinput2').classList.remove("is-valid");
    document.getElementById('textinput3').classList.remove("is-invalid");
    document.getElementById('textinput3').classList.remove("is-valid");
    window.history.replaceState('generator', document.title, "https://bot.hse-se.ru/");
    document.getElementById('button2id').blur();
}

function Ok() {
    event.preventDefault();
    document.getElementById('button1id').blur();
    if (!Check1(true) | !Check2(true) | !Check3(true))
        return;

    var newUrl = new URL("https://bot.hse-se.ru/problemsgenerator");
    var newSearchParams = newUrl.searchParams;
    
    newSearchParams.set('paragraph', document.getElementById('selectbasic').value);

    newSearchParams.set('num', document.getElementById('textinput1').value);
    
    newSearchParams.set('tasks', document.getElementById('textinput2').value);

    if (document.getElementById('button1id').innerHTML != "Сгенерировать с новым ключом" && document.getElementById('textinput3').value != "")
        newSearchParams.set('seed', document.getElementById('textinput3').value);

    fetch(newUrl).then(async function(response) {
        if (response.ok) {
            var json = await response.json();

            document.getElementById('textinput3').value = json[0]['seed'] - 1;
            Check3();
            Update4();
            document.getElementById('button1id').innerHTML = "Сгенерировать с новым ключом";
            document.getElementById('key').innerHTML = "Ключ: " + (json[0]['seed'] - 1);
            document.getElementById('print').style.display = 'block';
            document.title = "Генератор задач   Параграф: " + document.getElementById('selectbasic').value +
                ", Вариантов: " + document.getElementById('textinput1').value +
                ", Заданий: " + document.getElementById('textinput2').value +
                ", Ключ: " + document.getElementById('textinput3').value;
            window.history.pushState('{"url":"' + document.URL + '"}', "", document.URL);
            var text = "";
            for (var i = 0; i < json.length; i++) {
                text += "<div class=\"container\" style=\"background-color:#" + (i%2 == 0?"ffffff":"f8f8f8") + "\"><h4 style=\"padding-top:2%\">Вариант " + (i + 1) + "</h4>\n";
                for (var j = 0; j < json[i]['tasks'].length; j++) {
                    if (json[i]['tasks'][j]['polynomials'].length <= 2) {
                        text += "<b>Задание " + (j + 1) + ". </b>" + json[i]['tasks'][j]['task'] + "\n<div class=\"row\" ><div class=\"col-lg-6\" ><div class=\"row\" >";
                        for (var k = 0; k < json[i]['tasks'][j]['polynomials'].length; k++) {
                            text += "<div class=\"col-sm-6\" style=\"padding-bottom:1.2%;padding-top:0.5%\">" + (k + 1) + ") " + json[i]['tasks'][j]['polynomials'][k]['latex'] + "</div>";
                        }
                        text += "</div></div></div>";
                    } else {
                        text += "<b>Задание " + (j + 1) + ". </b>" + json[i]['tasks'][j]['task'] + "\n<div class=\"row\" ><div class=\"col-sm-6\" ><div class=\"row\" >";
                        for (var k = 0; k < json[i]['tasks'][j]['polynomials'].length; k++) {
                            if (k == 2) text += "</div></div><div class=\"col-sm-6\" ><div class=\"row\" >";
                            text += "<div class=\"col-lg-6\" style=\"padding-bottom:1.2%;padding-top:0.5%\">" + (k + 1) + ") " + json[i]['tasks'][j]['polynomials'][k]['latex'] + "</div>";
                        }
                        text += "</div></div></div>";
                    }
                }
                text += "</div>";
            }
            console.log(text);
            document.getElementById('latex').innerHTML = text; 
            MathJax.typeset();
        } else {
            alert("Ошибка на стороне сервера: " + response.status);
        }
    })
}

function Update1() {
    document.getElementById('button1id').innerHTML = "Сгенерировать";
    var url = new URL(document.URL);
    var search_params = url.searchParams;
    search_params.set("selectbasic", document.getElementById('selectbasic').value);
    window.history.replaceState('generator', document.title, url);
}

function Update2() {
    document.getElementById('button1id').innerHTML = "Сгенерировать";
    var url = new URL(document.URL);
    var search_params = url.searchParams;
    search_params.set("textinput1", document.getElementById('textinput1').value);
    window.history.replaceState('generator', document.title, url);
}
function Check1(strict = false) {
    document.getElementById('textinput1').classList.remove("is-invalid");
    document.getElementById('textinput1').classList.remove("is-valid");
    if (!strict && document.getElementById('textinput1').value === "") {
        return false;
    }
    var num = Number(document.getElementById('textinput1').value);
    if (isNaN(num) || num < 1 || num > 100) {
        document.getElementById('textinput1').classList.add("is-invalid");
        return false;
    } else {
        document.getElementById('textinput1').classList.add("is-valid");
        return true;
    }
}

function Update3() {
    document.getElementById('button1id').innerHTML = "Сгенерировать";
    var url = new URL(document.URL);
    var search_params = url.searchParams;
    search_params.set("textinput2", document.getElementById('textinput2').value);
    window.history.replaceState('generator', document.title, url);
}
function Check2(strict = false) {
    document.getElementById('textinput2').classList.remove("is-invalid");
    document.getElementById('textinput2').classList.remove("is-valid");
    if (!strict && document.getElementById('textinput2').value === "") {
        return false;
    }
    var num = Number(document.getElementById('textinput2').value);
    if (isNaN(num) || num < 1 || num > 20) {
        document.getElementById('textinput2').classList.add("is-invalid");
        return false;
    } else {
        document.getElementById('textinput2').classList.add("is-valid");
        return true;
    }
}

function Update4() {
    document.getElementById('button1id').innerHTML = "Сгенерировать";
    var url = new URL(document.URL);
    var search_params = url.searchParams;
    search_params.set("textinput3", document.getElementById('textinput3').value);
    window.history.replaceState('generator', document.title, url);
}
function Check3(strict = false) {
    document.getElementById('textinput3').classList.remove("is-invalid");
    document.getElementById('textinput3').classList.remove("is-valid");
    if (document.getElementById('textinput3').value === "") {
        return true;
    }
    var num = Number(document.getElementById('textinput3').value);
    if (isNaN(num) || num < 0 || num > 9999) {
        document.getElementById('textinput3').classList.add("is-invalid");
        return false;
    } else {
        document.getElementById('textinput3').classList.add("is-valid");
        return true;
    }
}