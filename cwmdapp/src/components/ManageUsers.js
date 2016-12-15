import React from 'react';
import cookie from 'react-cookie';
import User from "./User";
import $ from 'jquery';
import {Modal, Button} from "react-bootstrap";

export default class Home extends React.Component {
    constructor() {
        super();
        this.state = {
            users: [],
            roles: [],
            departments: [],
            showModal: false
        };
        this.addUser.bind(this);
        this.loadUsers.bind(this);
        this.open.bind(this);
        this.close.bind(this);
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

    loadDepartments() {
        $.ajax({
            url: "http://localhost:58879/api/departments",
            type: "get",
            headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + cookie.load('authToken')['access_token']},
            dataType: "json"
        }).then( result => {
            this.setState({departments: result});
        }).catch(err => {
            console.log(err);
        });
    }

    loadRoles() {
        $.ajax({
            url: "http://localhost:58879/api/roles",
            type: "get",
            headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + cookie.load('authToken')['access_token']},
            dataType: "json"
        }).then( result => {
            this.setState({roles: result});
        }).catch(err => {
            console.log(err);
        });
    }

    componentDidMount() {
        if (this.isLogged()) {
            this.loadUsers();
            this.loadDepartments();
            this.loadRoles();
        }
    }

    isLogged() {
        return cookie.load('authToken');
    }


    addUser() {
        let username = $("#username").val();
        let name = $("#name").val();
        let email = $("#email").val();
        let roleName = $("#createUserRole").val();
        let department = $("#createUserDepartment").val();

        let data = {
            username,
            name,
            email,
            roleName,
            department
        };

        $.ajax({
            url: "http://localhost:58879/api/accounts/create",
            type: "post",
            data: JSON.stringify(data),
            headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + cookie.load('authToken')['access_token']},
            dataType: "json"
        }).then( result => {
            this.loadUsers();
        }).catch(err => {
            console.log(err);
        });
    }

    deleteUser(id) {
        $.ajax({
            url: "http://localhost:58879/api/accounts/user/"+ id,
            type: "delete",
            headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + cookie.load('authToken')['access_token']}
        }).then( result => {
            console.log(result);
            this.loadUsers();
        }).catch(err => {
            console.log(err);
        });
    }

    close() {
        this.setState({showModal: false });
    }

    open() {
        this.setState({showModal: true });
    }

    onClickEdit() {
        this.open();
    }

    render() {
        if (this.isLogged())
            return (
                <div className="content-wrapper">
                    <h3>Manage users</h3>
                    <Button
                        bsStyle="primary"
                        onClick={() => this.open()}
                        value="Add user"
                    >Add User</Button>

                    <table className="table table-bordered users-table table-striped">
                        <thead>
                        <tr>
                            <th>Username</th>
                            <th>Name</th>
                            <th>Email</th>
                            <th>Department</th>
                            <th>Role</th>
                        </tr>
                        </thead>
                        <tbody>
                        {this.state.users.map(user => (
                                <User key={user.id}
                                      userName={user.userName}
                                      name={user.name}
                                      email={user.email}
                                      department={user.department}
                                      roles={user.roles}
                                      onClickEdit={() => this.props.onClickEdit(user)}
                                      onClickDelete={() => this.deleteUser(user.id)}
                                />
                            )
                        )}
                        </tbody>
                    </table>

                    <Modal show={this.state.showModal} onHide={() => this.close()}>
                        <Modal.Header closeButton>
                            <Modal.Title>Add user:</Modal.Title>
                        </Modal.Header>

                        <Modal.Body>
                            <form>
                                <div className="form-group">
                                    <input className="form-control" type="text" id="username" name="username" placeholder="Username"/>
                                </div>
                                <div className="form-group">
                                    <input className="form-control" type="text" id="name" name="name" placeholder="Name"/>
                                </div>
                                <div className="form-group">
                                    <input className="form-control" type="text" id="email" name="email" placeholder="Email"/>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="department">Department</label>
                                    <select className="form-control" name="department" id="createUserDepartment">
                                        {this.state.departments.map(department => (
                                            <option key={department.departmentID} value={department.name}>{department.name}</option>
                                        ))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label htmlFor="role">Role</label>
                                    <select className="form-control" name="role" id="createUserRole">
                                        {this.state.roles.map(role => (
                                            <option key={role.id} value={role.name}>{role.name}</option>
                                        ))}
                                    </select>
                                </div>
                            </form>
                        </Modal.Body>

                        <Modal.Footer>
                            <Button onClick={() => this.close()}>Close</Button>
                            <Button bsStyle="primary" onClick={() => {this.close(); this.addUser();}}>Save changes</Button>
                        </Modal.Footer>
                    </Modal>
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
