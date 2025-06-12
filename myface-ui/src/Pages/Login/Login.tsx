import React, {FormEvent, useContext, useState} from 'react';
import {Page} from "../Page/Page";
import {LoginContext} from "../../Components/LoginManager/LoginManager";
import "./Login.scss";

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext);
    
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    async function tryLogin(event: FormEvent) {
        event.preventDefault();
         try {
            loginContext.encodedLoginDetails(username, password);
            loginContext.logIn();
         
// IF response from Basic Auth Handler is 200, Login COntext.log > encode details 
        } catch (error) {
            loginContext.logOut();
            loginContext.header = "";
// Reload login and set logOUt context / encodeLoginDetails /relevant property should be null 
        }

        // Get response from handler and if returns 401, return to Loginpage, loginContext.logOut


    }
    
    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            <form className="login-form" onSubmit={tryLogin}>
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={username} onChange={event => setUsername(event.target.value)}/>
                </label>

                <label className="form-label">
                    Password
                    <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)}/>
                </label>
                
                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}