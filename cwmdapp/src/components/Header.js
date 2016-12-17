import React from 'react';
import { Link } from 'react-router';
import "../styles/main.css";
import cookie from 'react-cookie';

export default class Header extends React.Component {

    isLogged() {
        if(cookie.load('authToken')) {
            return true;
        }
        return false;
    }

    logout() {
        cookie.remove('authToken', '/');
    }

    render() {
        return (
            <div>
                <nav className="navbar navbar-default">
                    <div className="container-fluid">
                        <div className="nav-wrapper">
                            <div className="navbar-header">
                                <button type="button" className="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
                                    <span className="sr-only">Toggle navigation</span>
                                    <span className="icon-bar"></span>
                                    <span className="icon-bar"></span>
                                    <span className="icon-bar"></span>
                                </button>
                                <li><Link to="/home" className="navbar-brand">CWMD</Link></li>
                            </div>

                            <div className="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                                <ul className="nav navbar-nav">
                                    <li><Link to="/home">Home <span className="sr-only">(current)</span></Link></li>
                                    <li><Link to="/manage-users">Manage Users <span className="sr-only">(current)</span></Link></li>
                                    <li><Link to="/workspace">Workspace <span className="sr-only">(current)</span></Link></li>
                                </ul>
                                <ul className="nav navbar-nav navbar-right">
                                    <li><Link to="/login" className={(this.isLogged() ? 'hide' : 'show')}>Login</Link></li>
                                    <li><Link to="/login" className={(this.isLogged() ? 'show' : 'hide')} onClick={this.logout}>Logout</Link></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </nav>
            </div>
        );
    }
};
