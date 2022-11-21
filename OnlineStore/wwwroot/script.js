let needToHide = false;
window.onmousedown = function(event) {
    const modal = document.getElementsByClassName('modal')[0];
    if (event.target == modal) {
        needToHide = true;
    }
}        
window.onmouseup = function(event){
    const modal = document.getElementsByClassName('modal')[0];
    if(event.target == modal && needToHide){
        modal.removeChild(document.getElementsByClassName('accountForm')[0]);
        modal.style.display = 'none';
    }
    else needToHide = false;
}

function createSignInForm(){
    const modal = document.getElementsByClassName('modal')[0];
    modal.style.display = 'block';
    const form = document.createElement('form');
    form.className = 'accountForm';

    const closeSpan = document.createElement('span');
    closeSpan.onclick=()=>{
        modal.removeChild(document.getElementsByClassName('accountForm')[0]);
        modal.style.display = 'none';
    };
    closeSpan.className = 'formClose';
    closeSpan.id = 'formCloseSpan';
    closeSpan.innerText = '╳';

    const loginLabel = document.createElement('label');
    loginLabel.className = 'formLabel';
    loginLabel.id = 'formLoginLabel';
    loginLabel.innerText = 'Login';
    
    const loginInput = document.createElement('input');
    loginInput.className = 'formInput';
    loginInput.id = 'formLoginInput';

    const passwordLabel = document.createElement('label');
    passwordLabel.className = 'formLabel';
    passwordLabel.id = 'formPasswordLabel';
    passwordLabel.innerHTML = 'Password';

    const passwordInput = document.createElement('input');
    passwordInput.className = 'formInput';
    passwordInput.id = 'formPasswordInput';

    const logInButton = document.createElement('button');
    logInButton.className = 'formLogInButton';
    logInButton.addEventListener('click',logIn);
    logInButton.innerText = 'Log In';

    const registrationButton = document.createElement('button');
    registrationButton.className = 'formRegButton';
    registrationButton.addEventListener('click', registrationButtonClick);
    registrationButton.innerText = 'Register';

    form.appendChild(closeSpan);
    form.appendChild(loginLabel);
    form.appendChild(loginInput);
    form.appendChild(passwordLabel);
    form.appendChild(passwordInput);
    form.appendChild(logInButton);
    form.appendChild(registrationButton);

    modal.appendChild(form);
}

function registrationButtonClick(e){
    e.preventDefault();
    const form = document.getElementsByClassName('accountForm')[0];
    const logInButton = document.getElementsByClassName('formLogInButton')[0];
    form.removeChild(logInButton);
    
    const nameLabel = document.createElement('label');
    nameLabel.className = 'formLabel';
    nameLabel.id = 'formNameLabel';
    nameLabel.innerText = 'Name';
     
    const nameInput = document.createElement('input');
    nameInput.className = 'formInput';
    nameInput.id = 'formNameInput';

    const emailLabel = document.createElement('label');
    emailLabel.className = 'formLabel';
    emailLabel.id = 'formEmailLabel';
    emailLabel.innerText = 'Email';
     
    const emailInput = document.createElement('input');
    emailInput.className = 'formInput';
    emailInput.id = 'formEmailInput';

    const phoneLabel = document.createElement('label');
    phoneLabel.className = 'formLabel';
    phoneLabel.id = 'formPhoneLabel';
    phoneLabel.innerText = 'Phone';
     
    const phoneInput = document.createElement('input');
    phoneInput.id = 'formPhoneInput';
    phoneInput.className = 'formInput';

    const passwordInput = document.getElementById('formPasswordInput');
    passwordInput.after(nameLabel, nameInput, emailLabel, emailInput, phoneLabel, phoneInput);

    const backSpan = document.createElement('span');
    backSpan.onclick=()=>{
        form.removeChild(nameLabel);
        form.removeChild(nameInput);
        form.removeChild(emailInput);
        form.removeChild(emailLabel);
        form.removeChild(phoneInput);
        form.removeChild(phoneLabel);
        form.removeChild(backSpan);

        const loginLabel = document.createElement('label');
        loginLabel.className = 'formLabel';
        loginLabel.id = 'formLoginLabel';
        loginLabel.innerText = 'Login';

        const loginInput = document.createElement('input');
        loginInput.className = 'formInput';
        loginInput.id = 'formLoginInput';
        
        const logInButton = document.createElement('button');
        logInButton.className = 'formLogInButton';
        logInButton.addEventListener('click',logIn);
        logInButton.innerText = 'Log In';

        passwordInput.after(logInButton);

        registrationButton.removeEventListener('click', registration);
        registrationButton.addEventListener('click', registrationButtonClick);
    };
    backSpan.className = 'formBack';
    backSpan.innerText = '←';
    form.appendChild(backSpan);
    const registrationButton = document.getElementsByClassName('formRegButton')[0];
    registrationButton.removeEventListener('click', registrationButtonClick);
    registrationButton.addEventListener('click', registration);
}

async function logIn(e){
    e.preventDefault();

    let login = document.getElementById("formLoginInput").value;
    let password = document.getElementById("formPasswordInput").value;
    if(login!=='' && password !==''){
        const response = await fetch("/clients/authorisation", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                id:'null',
                login: login,
                password: password
            })
        });
        if (response.ok === true && response.status!=='401') {
            const data = await response.json();
            sessionStorage.setItem("JwtToken", data.access_token);
            sessionStorage.setItem("id", data.user.id);

            const modal = document.getElementsByClassName('modal')[0];
            modal.removeChild(document.getElementsByClassName('accountForm')[0]);
            modal.style.display = 'none';

            const logInButton = document.getElementsByClassName('SignInButton')[0];
            logInButton.style.display = 'none';
        }
        else  // если произошла ошибка, получаем код статуса
            console.log("Status: ", response.status);
    }
    else alert('Введите данные');
    
}

async function registration(e){
    e.preventDefault();

    let login = document.getElementById('formLoginInput').value;
    let password = document.getElementById('formPasswordInput').value;
    let name = document.getElementById('formNameInput').value;
    let email = document.getElementById('formEmailInput').value;
    let phone = document.getElementById('formPhoneInput').value;
    if(login !== '' && password !== '' && name !== '' && email !== '' && phone !== ''){
        const response = await fetch("/clients/registration", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                regUSer: {id:'null', password: password, login: login},
                user: {id:'null', name: name, email: email, phone: phone}
            })
        });
        if (response.ok === true && response.status!=='401') {
            const data = await response.json();
            sessionStorage.setItem("JwtToken", data.access_token);
            sessionStorage.setItem("id", data.user.id);
        }
        else  // если произошла ошибка, получаем код статуса
            console.log("Status: ", response.status);
    }
    else  alert('Введите данные');
}

async function updateAccount(e){
    e.preventDefault();

    const loginInput = document.getElementById('formLoginInput');
    const passwordInput = document.getElementById('formPasswordInput');
    const nameInput = document.getElementById('formNameInput');
    const emailInput = document.getElementById('formEmailInput');
    const phoneInput = document.getElementById('formPhoneInput');
    const id = sessionStorage.getItem('id');
    const token = sessionStorage.getItem('JwtToken');
    if(loginInput.value !== '' && passwordInput.value !== '' && nameInput.value !== '' && emailInput.value !== '' && phoneInput.value !== '' && id!==null && token!==null){
        const response = await fetch("/clients/update", {
            method: "PUT",
            headers: { "Accept": "application/json", "Content-Type": "application/json", "Authorization": "Bearer " + token },
            body: JSON.stringify({
                regUSer: {id: id, password: passwordInput.value, login: loginInput.value},
                user: {id: id, name: nameInput.value, email: emailInput.value, phone: phoneInput.value}
            })
        });
        if (response.ok === true && response.status!=='401') {
            const editButton = document.getElementsByClassName('formEditButton')[0];
            editButton.style.display = 'block';

            emailInput.readOnly = false;
            nameInput.readOnly = false;
            phoneInput.readOnly = false;

            const form = document.getElementsByClassName('accountForm')[0];
            const saveButton = document.getElementsByClassName('formSaveButton')[0];
            const deleteButton = document.getElementsByClassName('formDeleteButton')[0];
            const cancelButton = document.getElementsByClassName('formCancelButton')[0];

            const loginLabel = document.getElementById('formLoginLabel');
            const passwordLabel = document.getElementById('formPasswordLabel');

            form.removeChild(saveButton);
            form.removeChild(deleteButton);
            form.removeChild(cancelButton);
            form.removeChild(loginInput);
            form.removeChild(loginLabel);
            form.removeChild(passwordInput);
            form.removeChild(passwordLabel);

            alert('Аккаунт успешно изменен');
        }
        else  // если произошла ошибка, получаем код статуса
            console.log("Status: ", response.status);
    }
    else  alert('Введите данные');
}

async function deleteAccount(e){
    e.preventDefault();

    const id = sessionStorage.getItem('id');
    const token = sessionStorage.getItem('JwtToken');
    if(id!==null){
        const response = await fetch("/clients/"+id, {
            method: "DELETE",
            headers: { "Accept": "application/json", "Content-Type": "application/json", "Authorization": "Bearer " + token},
        });
        if (response.ok === true && response.status!=='401') {
            alert('Аккаунт успешно удален');
            const modal = document.getElementsByClassName('modal')[0];
            modal.removeChild(document.getElementsByClassName('accountForm')[0]);
            modal.style.display = 'none'; 
        }
        else  // если произошла ошибка, получаем код статуса
            console.log("Status: ", response.status);
    }
}

async function getAccount(){
    let token = sessionStorage.getItem('JwtToken');
    let userId = sessionStorage.getItem('id');
    if(token !== null && userId !== null){
        

        const response = await fetch("/clients/"+userId, {
            method: "GET",
            headers: { "Accept": "application/json", "Content-Type": "application/json", "Authorization": "Bearer " + token }
        });
        if (response.ok === true && response.status!=='401') {
            return await response.json();
        }
        else {
            console.log("Status: ", response.status);
            return null;
        }
    }
    else return null;
    
}

async function createProfileForm(){
    let account = await getAccount();
    console.log(account);
    if(account!==null){
        const modal = document.getElementsByClassName('modal')[0];
        modal.style.display = 'block';
        const form = document.createElement('form');
        form.className = 'accountForm';

        const closeSpan = document.createElement('span');
        closeSpan.onclick=()=>{
            modal.removeChild(document.getElementsByClassName('accountForm')[0]);
            modal.style.display = 'none';
        };
        closeSpan.className = 'formClose';
        closeSpan.id = 'formCloseSpan';
        closeSpan.innerText = '╳';

        const nameLabel = document.createElement('label');
        nameLabel.className = 'formLabel';
        nameLabel.id = 'formNameLabel';
        nameLabel.innerText = 'Name';
        
        const nameInput = document.createElement('input');
        nameInput.className = 'formInput';
        nameInput.id = 'formNameInput';
        nameInput.value = account.user.name;
        nameInput.readOnly = true;

        const emailLabel = document.createElement('label');
        emailLabel.className = 'formLabel';
        emailLabel.id = 'formEmailLabel';
        emailLabel.innerText = 'Email';
        
        const emailInput = document.createElement('input');
        emailInput.className = 'formInput';
        emailInput.id = 'formEmailInput';
        emailInput.value = account.user.email;
        emailInput.readOnly = true;

        const phoneLabel = document.createElement('label');
        phoneLabel.className = 'formLabel';
        phoneLabel.id = 'formPhoneLabel';
        phoneLabel.innerText = 'Phone';
        
        const phoneInput = document.createElement('input');
        phoneInput.id = 'formPhoneInput';
        phoneInput.className = 'formInput';
        phoneInput.value = account.user.phone;
        phoneInput.readOnly = true;

        const editButton = document.createElement('button');
        editButton.className = 'formEditButton';
        editButton.innerHTML = 'Edit profile';
        editButton.addEventListener('click',(e)=>{
            e.preventDefault();

            const name = nameInput.value;
            const email = emailInput.value;
            const phone = phoneInput.value;

            nameInput.readOnly = false;
            emailInput.readOnly = false;
            phoneInput.readOnly = false;

            const loginLabel = document.createElement('label');
            loginLabel.className = 'formLabel';
            loginLabel.id = 'formLoginLabel';
            loginLabel.innerText = 'Login';
            
            const loginInput = document.createElement('input');
            loginInput.className = 'formInput';
            loginInput.id = 'formLoginInput';
            loginInput.value = account.regUser.login;
        
            const passwordLabel = document.createElement('label');
            passwordLabel.className = 'formLabel';
            passwordLabel.id = 'formPasswordLabel';
            passwordLabel.innerHTML = 'Password';
        
            const passwordInput = document.createElement('input');
            passwordInput.className = 'formInput';
            passwordInput.id = 'formPasswordInput';
            passwordInput.value = account.regUser.password;

            const saveButton = document.createElement('button');
            saveButton.className = 'formSaveButton';
            saveButton.innerHTML = 'Save profile changes';
            saveButton.addEventListener('click', updateAccount)

            const cancelButton = document.createElement('button');
            cancelButton.className = 'formCancelButton';
            cancelButton.innerHTML = 'Cancel';
            cancelButton.addEventListener('click', (e)=>{
                e.preventDefault();

                editButton.style.display = 'block';

                emailInput.value = email;
                nameInput.value = name;
                phoneInput.value = phone;

                emailInput.readOnly = false;
                nameInput.readOnly = false;
                phoneInput.readOnly = false;

                form.removeChild(saveButton);
                form.removeChild(deleteButton);
                form.removeChild(cancelButton);
                form.removeChild(loginInput);
                form.removeChild(loginLabel);
                form.removeChild(passwordInput);
                form.removeChild(passwordLabel);
            });

            const deleteButton = document.createElement('button');
            deleteButton.className = 'formDeleteButton';
            deleteButton.innerHTML = 'Delete profile';
            deleteButton.addEventListener('click', deleteAccount);

            

            editButton.before(loginLabel,loginInput, passwordLabel, passwordInput);
            editButton.after(saveButton, cancelButton, deleteButton);
            editButton.style.display = 'none';
        });

        const logOutButton = document.createElement('button');
            logOutButton.className = 'formLogOutButton';
            logOutButton.innerHTML = 'Log Out';
            logOutButton.addEventListener('click', (e)=>{
            e.preventDefault();

            sessionStorage.removeItem('JwtToken');
            sessionStorage.removeItem('id');

            const modal = document.getElementsByClassName('modal')[0];
            modal.removeChild(document.getElementsByClassName('accountForm')[0]);
            modal.style.display = 'none';

            const logInButton = document.getElementsByClassName('SignInButton')[0];
            logInButton.style.display = 'initial';
        });

        form.appendChild(closeSpan);
        form.appendChild(nameLabel);
        form.appendChild(nameInput);
        form.appendChild(emailLabel);
        form.appendChild(emailInput);
        form.appendChild(phoneLabel);
        form.appendChild(phoneInput);
        form.appendChild(editButton);
        form.appendChild(logOutButton);

        modal.appendChild(form);
    }
    else alert('Вы не вошли в аккаунт');
}
   
