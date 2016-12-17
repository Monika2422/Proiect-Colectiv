import React, { Component } from 'react';
import '../styles/main.css';

export default class User extends Component {
    render() {
        return (
            <tr>
                <td>{this.props.userName}</td>
                <td>{this.props.name}</td>
                <td>{this.props.email}</td>
                <td>{this.props.department}</td>
                <td>{this.props.roles[0]}</td>
                <td onClick={this.props.onClickDelete}>
                    <a href="javascript:void(0);" className="btn btn-danger">Delete</a>
                </td>
            </tr>
        );
    }
}
