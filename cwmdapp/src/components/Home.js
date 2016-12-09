import React from 'react';
import cookie from "react-cookie";

export default class Home extends React.Component {

    isLogged() {
        if(cookie.load('authToken')) {
            return true;
        }
        return false;
    }

    render() {
        if (this.isLogged())
            return (
                <div className="content-wrapper">
                    <h3>You are logged in. This is the homepage.</h3>
                </div>
            );
        else
            return (
                <div className="content-wrapper">
                    <h3>You are not logged in</h3>
                </div>
            );
    }
};
