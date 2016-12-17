/**
 * Created by Isabella on 12/15/2016.
 */
import React, { Component } from 'react';
import '../styles/main.css';

export default class Document extends Component {
    render() {
        return (
            <tr onClick={this.props.bigComponent.handleClick(this.props.key)}>
                <td>{this.props.fileName}</td>
                <td>{this.props.creationDate}</td>
            </tr>
        );
    }
}