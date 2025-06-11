import React, {createContext, ReactNode, useState} from "react";
//import {fetchUserDetails, GetUserInputHashed} from "../../Api/apiClient";


export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => {},
    logOut: () => {}
    // checkLoginDetails: async (username: string, password: string) => Promise.resolve(false)
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(true);
    
    function logIn() {
        setLoggedIn(true);
    }

    function logOut() {
        setLoggedIn(false);
    }

    // async function checkLoginDetails(username: string, password: string) {
    //     // console.log(password)
    //     var response = await fetchUserDetails(username);
    //     var user = response.items[0];
    //     var storedUsername = user.username;
    //     var storedHashedPassword = user.hashedPassword;
    //     var storedSalt = user.salt;
    //     console.log(typeof storedSalt);

    //     //storedSalt = Array.ConvertAll(bytesArray, Convert.ToInt32);

    //     // console.log(typeof storedSalt);
    //     var hashedPassword = await GetUserInputHashed(password, storedSalt);
    //     console.log(hashedPassword);
    //     // var hashedPassword = await GetUserInputHashed(password, storedSalt);

    //     // return hashedPassword == storedHashedPassword && username == storedUsername ? true : false;
    //     return false;
    // }

    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut
        // checkLoginDetails: checkLoginDetails
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}