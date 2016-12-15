import React from 'react';
import "../styles/main.css";
import $ from "../../node_modules/jquery/dist/jquery";
import { browserHistory} from 'react-router';
import cookie from 'react-cookie';

export default class Login extends React.Component {
    render() {
        return (
            <div className="content-wrapper">
                <div className="login-container">
                    <h1>Login to Your Account</h1>
                    <form>
                        <input type="text" id="user" name="user" placeholder="Username"/>
                        <input type="password" id="pass" name="pass" placeholder="Password"/>
                        <input type="button" name="login" className="login login-submit" value="Login" onClick={this.login}/>
                    </form>
                </div>
                {this.props.children}
            </div>
        );
    }

    login() {
        let username = $("#user").val();
        let password = $("#pass").val();

        $.ajax({
            url: "http://localhost:58879/oauth/token",
            type: "post",
            headers: {'Content-Type': 'application/x-www-form-urlencoded'},
            data: $.param({username: username, password: password, grant_type: "password"}),
            dataType: "json"
        }).then( result => {
            console.log(result);
            cookie.save('authToken', result, { path: '/' });
            const path = `/home`;
            browserHistory.push(path);
        }).catch(err => {
            console.log(err);
        });
    }
};
