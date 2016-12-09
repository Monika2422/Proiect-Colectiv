import React, { Component } from 'react';
import './styles/main.css';
import Header from "./components/Header";

class App extends Component {
    render() {
        return (
            <div>
                <Header />
                {this.props.children}
            </div>
        );
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            children: nextProps.children
        });
    }
}

export default App;
