import React, {createContext, ReactNode, useState} from "react";
//import {fetchUserDetails, GetUserInputHashed} from "../../Api/apiClient";


export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => {},
    logOut: () => {},
    encodedLoginDetails: (username:string, password: string) => {},
    header: ""
    // checkLoginDetails: async (username: string, password: string) => Promise.resolve(false)
    
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [header, setHeader] = useState("");
    
    function logIn() {
        setLoggedIn(true);
    }

    function logOut() {
        setLoggedIn(false);
    }

    function getLoginDetails(username:string, password: string) {

        // Combine username and password with a colon
         const credentials = `${username}:${password}`;

        // Encode the credentials to Base64
        const base64EncodedCredentials = btoa(credentials);

        // Add to the Authorization header
        const authorizationHeader = `Basic ${base64EncodedCredentials}`;
        setHeader(authorizationHeader);
        return authorizationHeader;
    }

    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
        encodedLoginDetails: getLoginDetails,
        header: header
        // checkLoginDetails: checkLoginDetails
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}