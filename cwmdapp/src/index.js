import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './styles/main.css';
import { Router, Route, browserHistory } from 'react-router';
import Home from "./components/Home";
import Login from "./components/Login";
import ManageUsers from "./components/ManageUsers";

ReactDOM.render(
    <Router history={browserHistory}>
        <Route path="/" component={App}>
            <Route path="/home" component={Home}/>
            <Route path="/login" component={Login}/>
            <Route path="/manage-users" component={ManageUsers}/>
        </Route>
    </Router>,
    document.getElementById('root')
);
