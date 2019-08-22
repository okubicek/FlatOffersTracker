import React, { Component } from 'react';

export class SelectBox extends Component {
    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e) {
        this.props.onChange(e.target);
    }

    renderOptions() {
        return (
            this.props.options.map(val => 
                <option>{val}</option>
            )
        );
    }

    render() {
        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                <select
                    className="form-control"
                    value={this.props.value}
                    onChange={this.handleChange}
                >
                    this.renderOptions()
                </select>
            </div>
        );
    }
}