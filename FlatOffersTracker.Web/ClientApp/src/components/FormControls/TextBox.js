import React, { Component } from 'react';
export class TextBox extends Component {
    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e) {
        this.props.OnChange(e.target);
    }

    render() {
        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                <input type="text"
                    value={this.props.value}
                    onChange={this.handleChange}
                    className="form-control"
                />
            </div>
        );
    }
}