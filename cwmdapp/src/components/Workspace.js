/**
 * Created by Isabella on 12/15/2016.
 */
import React from 'react';
import cookie from "react-cookie";
import $ from "../../node_modules/jquery/dist/jquery";

export default class Workspace extends React.Component {


    constructor() {
        super();
        this.state = {
            documents: [],
            documentsSize: 0
        };
        this.loadDocuments.bind(this);
        this.loadVersions.bind(this);
    }

    loadDocuments() {
        var username = cookie.load('username');
        console.log("Username"+username);
        $.ajax({
            url: "http://localhost:58879/api/docs/documents/"+username,
            type: "get",
            headers: {'Content-Type': 'application/json'},
            dataType: "json"
        }).then( result => {
            console.log("result found")
            console.log(result);
            this.setState({documents: result});
            this.setState({documentsSize: result.length});
        }).catch(err => {
            console.log(err);
        });
    }

    loadVersions() {
        $.ajax({
            url: "http://localhost:58879/api/ver/versions",
            type: "get",
            headers: {'Content-Type': 'application/json'},
            dataType: "json"
        }).then( result => {
            console.log("result found")
            console.log(result);
            this.setState({versions: result});
        }).catch(err => {
            console.log(err);
        });
    }

    componentWillMount() {
        if (this.isLogged()) {
            this.loadDocuments();
            this.loadVersions();
        }
    }

    isLogged() {
        if(cookie.load('authToken')) {
            return true;
        }
        return false;
    }

    handleClick(ind){
        $( "#textdiv" ).css('white-space','pre-wrap');
        console.log("Change");
        for(var i = 0; i < this.state.documents.length; i++) {
            var obj = this.state.documents[i];
            //console.log(obj.id);
            if(obj.id===ind){
                console.log("found");
                console.log(obj.status+"|"+obj.abstract+"|"+obj.keyWords);
                $('#status').html(obj.status);
                $('#abstract').html(obj.abstract);
                $('#keyWords').html(obj.keyWords);
            }
        }

        $('#versionsdiv').html('');
        $('#versionsdiv').append( "Versions" );
        for(var i = 0; i < this.state.versions.length; i++) {
            var obj = this.state.versions[i];
            if(obj.documentId===ind){
                var domId="ver"+obj.id;
                var str="<p id='" + "ver" + obj.id + "'>" +obj.versionNumber+"</p>";
                $( "#versionsdiv" ).append(str);
                $( "#textdiv" ).html(obj.text.substring(0, obj.text.length - 1));
            }
        }

    }

    render() {

        var tdstyle = {
            width: '100px',
            verticalAlign: 'text-top',
            padding: '5px'
        };

        if (this.isLogged())
            return (
                <div className="content-wrapper">
                    <table className="table">
                        <tr>
                            <td style={tdstyle}>
                                <h4>Abstract: </h4><div id="abstract"></div>
                                <h4>Status: </h4><div id="status"></div>
                                <h4>Key Words: </h4><div id="keyWords"></div>
                            </td>

                            <td style={tdstyle}>
                                <div id="versionsdiv"></div>
                                <div className="table-bordered" id="textdiv"></div>
                            </td>

                            <td style={tdstyle}>
                                <table className="table table-bordered">
                                    <thead>
                                    <tr>
                                        <th>File Name</th>
                                        <th>Creation Date</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    {
                                        this.state.documents.map(document => (
                                            <tr onClick={() => this.handleClick(document.id)}>
                                                <td>{document.fileName}</td>
                                                <td>{document.creationDate}</td>
                                            </tr>
                                        ))
                                    }
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>

                </div>
            );
        else
            return (
                <div className="content-wrapper">
                    <h3>You are not logged in</h3>
                </div>
            );
    }
};