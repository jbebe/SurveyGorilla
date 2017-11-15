/// <reference path="../lib/jquery/dist/jquery.js" />
/// <reference path="../lib/js-cookie/src/js.cookie.js" />

Cookies.get('.AspNetCore.Session') !== undefined ? LoadView('survey-list') : LoadView('login');

function LoadView(viewName, ...loadFnArgs) {
    const suffix = '-view';
    $(`div[id$="${suffix}"]`).each((index, element) => {
        let view = $(element);
        if (view.prop('id') === viewName + suffix) {
            view.show();
            view.find('*[data-load]').each((index, element) => {
                window[$(element).data('load')].call(element, ...loadFnArgs);
            });
        } else {
            view.hide();
        }
    });
}

function Log(message) {
    let log = $('#message-log');
    log.text(`${log.text()}\n${message}`);
}

function AjaxSuccessHandler(callback = () => { }) {
    return (data, textStatus, jqXHR) => {
        callback(data, textStatus, jqXHR);
        Log(`${textStatus}, ${JSON.stringify(jqXHR)}`);
    };
}

function AjaxErrorHandler(callback = () => { }) {
    return (jqXHR, textStatus, errorThrown) => {
        callback(jqXHR, textStatus, errorThrown);
        Log(`${JSON.stringify(jqXHR)}, ${textStatus}, ${errorThrown}`);
    };
}

function LoginAction(evt) {
    evt.preventDefault();

    let form = $(this);
    let email = form.find('input[type=email]').val();
    let password = form.find('input[type=password]').val();
    let data = {
        email: email,
        password: password
    };
    let onSuccess = () => {
        LoadView('survey-list');
    };
    $.ajax({
        url: '/login',
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
    })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());

    return false;
}

function LogoutAction() {
    let onSuccess = () => {
        LoadView('login');
    };
    $.ajax({
        url: '/logout',
        type: 'GET'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
}

function RegisterAction(evt) {
    evt.preventDefault();

    let form = $(this);
    let email = form.find('input[type=email]').val();
    let password = form.find('input[type=password]').val();
    let data = {
        Email: email,
        Password: password,
        Info: JSON.stringify({})
    };
    let onSuccess = () => {
        LoadView('login');
    };
    $.ajax({
        url: '/register',
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json"
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
    return false;
}

function LoadSurveyList() {
    let list = $(this);
    list.empty();
    let onSuccess = surveys => {
        surveys.forEach(survey => {
            let info = JSON.parse(survey.info);
            list.append(
                `<li>
                    <button onclick="LoadView('survey', ${survey.id})">
                        ${survey.name}(${survey.id}) -> ${info.created}
                    </button>
                    <button onclick="DeleteSurveyAction.call(this, ${survey.id})">X</button>
                </li>`
            );
        });
    };
    $.ajax({
        url: '/api/Survey',
        type: 'GET'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
}

function CreateSurveyAction(evt) {
    evt.preventDefault();

    let form = $(this);
    let name = form.find('input[type=text]').val();
    let data = {
        name: name,
        info: JSON.stringify({})
    };
    let onSuccess = () => {
        LoadView('survey-list');
    };
    $.ajax({
        url: '/api/Survey',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());

    return false;
}

function DeleteSurveyAction(surveyId) {
    let onSuccess = () => {
        App.refresh();
    };
    $.ajax({
        url: `/api/Survey/${surveyId}`,
        type: 'DELETE'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
}

function LoadSurveyDetails(surveyId) {
    let list = $(this);
    list.empty();
    let onSuccess = data => {
        list.append(`
            <li>Id: ${data.id}</li>
            <li>AdminId: ${data.adminId}</li>
            <li>Info: ${data.info}</li>
        `);
    };
    $.ajax({
        url: `/api/Survey/${surveyId}`,
        type: 'GET'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
}

function LoadSurveyQuestions(surveyId) {
    let list = $(this);
    list.empty();
    let onSuccess = data => {
        let questions = JSON.parse(data.info).questions;
        questions.forEach(question => {
            list.append(`<li>Title: ${question.title}</li>`);
        });
    };
    $.ajax({
        url: `/api/Survey/${surveyId}`,
        type: 'GET'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
}

function LoadSurveyClients(surveyId) {
    let list = $(this);
    list.empty();
    let onSuccess = data => {
        data.forEach(client => {
            list.append(`<li>email: ${client.emailAddress}</li>`);
        });
    };
    $.ajax({
        url: `/api/Survey/${surveyId}/Client`,
        type: 'GET'
        })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());
}

function CreateQuestionAction(evt) {
    evt.preventDefault();

    let form = $(this);
    let question = form.find('input[type=text]:eq(0)').val();
    let answer = form.find('input[type=text]:eq(1)').val();
    let data = {
        info: JSON.stringify({
            questions: [{ question: question, answer: answer }]
        })
    };
    let onSuccess = () => {
        LoadView('survey-list');
    };
    $.ajax({
        url: '/api/Survey',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json'
    })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());

    return false;
}

/*
Question Structure:
[
    {
        type: "number"
        title: "What is your favorite number?"
        value: <default-number>
    },
    {
        type: "text"
        title: "What is your quest?"
        value: <default-string>
    },
    {
        type: "choose"
        title: "What is your favorite color?"
        value: ["blue", "yello", "brown"]
    },
    {
        type: "checkbox"
        title: "What is your favorite color?"
        value: ["blue", "yello", "brown"]
    }
]
*/
function CreateQuestionAction(evt) {
    evt.preventDefault();

    let form = $(this);
    let question = form.find('input[type=text]:eq(0)').val();
    let answer = form.find('input[type=text]:eq(1)').val();
    let data = {
        info: JSON.stringify({
            questions: [{ question: question, answer: answer }]
        })
    };
    let onSuccess = () => {
        LoadView('survey-list');
    };
    $.ajax({
        url: '/api/Survey',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json'
    })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());

    return false;
}

function CreateClientAction(evt) {
    evt.preventDefault();

    let form = $(this);
    let name = form.find('input[type=text]').val();
    let email = form.find('input[type=email]').val();
    let data = {
        email: email,
        info: JSON.stringify({
            name: name
        })
    };
    let onSuccess = () => {
        LoadView('survey-list');
    };
    $.ajax({
        url: '/api/Survey',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json'
    })
        .done(AjaxSuccessHandler(onSuccess))
        .fail(AjaxErrorHandler());

    return false;
}