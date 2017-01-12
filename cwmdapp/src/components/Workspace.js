/**
 * Created by Isabella on 12/15/2016.
 */
import React from 'react';
import cookie from "react-cookie";
import $ from "../../node_modules/jquery/dist/jquery";
import {Modal, Button} from "react-bootstrap";

var Dropzone = require('react-dropzone');

export default class Workspace extends React.Component {


    constructor() {
        super();
        this.state = {
            documents: [],
            documentsSize: 0,
            showModal: false,
            file: '',
            filePreviewUrl: ''
        };
        this.loadDocuments.bind(this);
        this.loadVersions.bind(this);
        this.open.bind(this);
        this.close.bind(this);
        this.handleFile.bind(this);
        this.handleSubmit.bind(this);
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

    close() {
        this.setState({showModal: false });
    }

    open() {
        this.setState({showModal: true });
    }

    handleFile(e) {
        e.preventDefault();

        let reader = new FileReader();
        let file = e.target.files[0];

        reader.onloadend = () => {
            this.setState({
                file: file,
                filePreviewUrl: reader.result
            });
        };

        reader.readAsDataURL(file);
        console.log("File received");
    }

    handleSubmit(e) {
        e.preventDefault();
        let abstract = $("#abstractInput").val();
        let keywords = $("#keywordsInput").val();
        let username = cookie.load('username');
        let uploadedFile = this.state.file;
        console.log('handle uploading-', this.state.file);
        console.log(abstract);
        console.log(keywords);

        var formData = new FormData();
        formData.append("UploadedFile",uploadedFile);
        formData.append("KeyWords",keywords);
        formData.append("Abstract",abstract);
        formData.append("Username",username);

        $.ajax({
            url: "http://localhost:58879/api/files/upload",
            type: "post",
            data: formData,
            Authorization: 'Bearer ' + cookie.load('authToken')['access_token'],
            dataType: 'json',
            cache: false,
            processData: false,
            contentType: false
        }).then( result => {
            this.loadDocuments();
            console.log("upload complete!");
        }).catch(err => {
            console.log(err);
        });
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
                    <h3>Workspace</h3>
                    <Button
                        bsStyle="primary"
                        onClick={() => this.open()}
                        value="Add document"
                    >Add Document</Button>
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
                    <Modal show={this.state.showModal} onHide={() => this.close()}>
                        <Modal.Header closeButton>
                            <Modal.Title>Add document:</Modal.Title>
                        </Modal.Header>

                        <Modal.Body>
                            <form>
                                <div className="form-group">
                                    <input className="form-control" type="text" id="abstractInput" name="abstract" placeholder="Abstract"/>
                                </div>
                                <div className="form-group">
                                    <input className="form-control" type="text" id="keywordsInput" name="keywords" placeholder="Key Words"/>
                                </div>
                                <div>
                                    <input className="fileInput" type="file" onChange={(e)=>this.handleFile(e)} />

                                </div>

                            </form>
                        </Modal.Body>

                        <Modal.Footer>
                            <Button onClick={() => this.close()}>Close</Button>
                            <Button bsStyle="primary"  onClick={(e)=>this.handleSubmit(e)}>Save</Button>
                        </Modal.Footer>
                    </Modal>
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