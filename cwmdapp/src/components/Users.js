import React from 'react';
import cookie from 'react-cookie';
import User from "./User";
import $ from 'jquery';
import {Modal, Button} from "react-bootstrap";

export default class  extends React.Component {
    render() {
        return (
            <div className="content-wrapper">
                <h3>Manage users</h3>
                <Button
                    bsStyle="primary"
                    onClick={() => this.props.openModal()}
                    value="Add user"
                >Add User</Button>

                <table className="table table-bordered users-table table-striped">
                    <thead>
                    <tr>
                        <th>Username</th>
                        <th>Name</th>
                        <th>Email</th>
                    </tr>
                    </thead>
                    <tbody>
                    {this.props.users.map(user => (
                            <User key={user.id}
                                  userName={user.userName}
                                  name={user.name}
                                  email={user.email}
                                  onClickEdit={() => this.props.onClickEdit(user)}
                                  onClickDelete={() => console.log("detele")}
                            />
                        )
                    )}
                    </tbody>
                </table>

                <Modal show={this.props.showModal} onHide={() => this.props.closeModal()}>
                    <Modal.Header closeButton>
                        <Modal.Title>Add user:</Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        <div className="row">
                            <div className="col-md-12">
                                <input className="form-control" type="text" id="username" name="username" placeholder="Username"/>
                            </div>
                            <div className="col-md-12">
                                <input className="form-control" type="text" id="name" name="name" placeholder="Name"/>
                            </div>
                            <div className="col-md-12">
                                <input className="form-control" type="text" id="email" name="email" placeholder="email"/>
                            </div>
                        </div>
                    </Modal.Body>

                    <Modal.Footer>
                        <Button onClick={() => this.close()}>Close</Button>
                        <Button bsStyle="primary" onClick={() => {this.close(); this.addUser();}}>Save changes</Button>
                    </Modal.Footer>
                </Modal>
            </div>
        );
    }
};
