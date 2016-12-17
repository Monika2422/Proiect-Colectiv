import React from 'react';
import $ from "../../node_modules/jquery/dist/jquery";
import cookie from 'react-cookie';
import User from "./User";

export default class Home extends React.Component {
    constructor() {
        super();
        this.state = {
            users: []
        };
        this.addUser.bind(this);
        this.loadUsers.bind(this);
    }

    loadUsers() {
        $.ajax({
            url: "http://localhost:58879/api/accounts/users",
            type: "get",
            headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + cookie.load('authToken')['access_token']},
            dataType: "json"
        }).then( result => {
            console.log(result);
            this.setState({users: result});
        }).catch(err => {
            console.log(err);
        });
    }

    componentDidMount() {
        if (this.isLogged()) {
            this.loadUsers();
        }
    }

    isLogged() {
        return cookie.load('authToken');
    }


    addUser() {
        let username = $("#username").val();
        let name = $("#name").val();
        let email = $("#email").val();

        let data = {
            username,
            name,
            email
        };

        $.ajax({
            url: "http://localhost:58879/api/accounts/create",
            type: "post",
            data: JSON.stringify(data),
            headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + cookie.load('authToken')['access_token']},
            dataType: "json"
        }).then( result => {
            console.log(result);
            console.log(this);
            this.loadUsers();
        }).catch(err => {
            console.log(err);
        });
    }

    render() {
        if (this.isLogged())
            return (
                <div className="content-wrapper">
                    <h3>Manage users</h3>
                    <div className="row">
                        <div className="col-md-4">
                            <input className="form-control" type="text" id="username" name="username" placeholder="Username"/>
                        </div>
                        <div className="col-md-4">
                            <input className="form-control" type="text" id="name" name="name" placeholder="Name"/>
                        </div>
                        <div className="col-md-4">
                            <input className="form-control" type="text" id="email" name="email" placeholder="email"/>
                        </div>
                        <input type="button" name="addUser" className="btn btn-default" value="Add" onClick={() => this.addUser()}/>
                    </div>


                    <table className="table table-bordered">
                        <thead>
                        <tr>
                            <th>Username</th>
                            <th>Name</th>
                            <th>Email</th>
                        </tr>
                        </thead>
                        <tbody>
                        {this.state.users.map(user => (
                                <User userName={user.userName} name={user.name} email={user.email} key={user.id}/>
                            )
                        )}
                        </tbody>
                    </table>
                </div>
            );
        else {
            return (
                <div className="content-wrapper">
                    <h3>You are not logged in</h3>
                </div>
            )
        }
    }
};
