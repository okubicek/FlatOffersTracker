import React, { Component } from 'react';
export class TextBox extends Component {
    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e) {
        this.props.onChange(e.target);
    }

    render() {
        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                <input type="text"
                    name={this.props.name}
                    value={this.props.value}
                    onChange={this.handleChange}
                    className="form-control"
                />
            </div>
        );
    }
}