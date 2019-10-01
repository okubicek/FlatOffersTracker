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
            this.props.options.map(opt =>
                <option key={opt.key} value={opt.value}>{opt.value}</option>
            )
        );
    }

    render() {
        return (
            <div className="form-group">
                <label>{this.props.label}</label>
                <select
                    name={this.props.name}
                    className="form-control"
                    value={this.props.value}
                    onChange={this.handleChange}
                >
                    <option></option>
                    {this.renderOptions()}
                </select>
            </div>
        );
    }
}