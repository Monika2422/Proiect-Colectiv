import React, { Component } from 'react';
import '../styles/main.css';

export default class User extends Component {
    render() {
        return (
            <tr>
                <td>{this.props.userName}</td>
                <td>{this.props.name}</td>
                <td>{this.props.email}</td>
            </tr>
        );
    }
}
